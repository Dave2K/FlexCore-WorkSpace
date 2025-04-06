using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using FlexCore.Caching.Core.Interfaces;

namespace FlexCore.Caching.Redis
{
    public class RedisCacheProvider : ICacheProvider
    {
        private readonly IConnectionMultiplexer _connection;
        private readonly ILogger<RedisCacheProvider> _logger;
        private readonly IDatabase _database;
        private readonly JsonSerializerOptions _serializerOptions;

        public RedisCacheProvider(
            string connectionString,
            ILogger<RedisCacheProvider> logger,
            IConnectionMultiplexer? connection = null,
            JsonSerializerOptions? serializerOptions = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serializerOptions = serializerOptions ?? new JsonSerializerOptions();

            try
            {
                _connection = connection ?? ConnectionMultiplexer.Connect(connectionString);
                _database = _connection.GetDatabase();
                _logger.LogInformation("Connesso a Redis: {Endpoints}", string.Join(", ", _connection.GetEndPoints().Select(e => e.ToString())));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Connessione fallita a Redis");
                throw new RedisConnectionException(ConnectionFailureType.UnableToConnect, "Errore di connessione a Redis", ex);
            }
        }

        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                return await _database.KeyExistsAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante ExistsAsync per {Key}", key);
                return false;
            }
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var redisValue = await _database.StringGetAsync(key);
                if (redisValue.IsNullOrEmpty)
                {
                    _logger.LogDebug("Chiave {Key} non trovata", key);
                    return default;
                }

                if (typeof(T) == typeof(string))
                    return (T)(object)redisValue.ToString();

                return JsonSerializer.Deserialize<T>(redisValue.ToString(), _serializerOptions);
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Deserializzazione fallita per {Key}", key);
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante GetAsync per {Key}", key);
                return default;
            }
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan expiry)
        {
            try
            {
                var serializedValue = JsonSerializer.Serialize(value, _serializerOptions);
                return await _database.StringSetAsync(key, serializedValue, expiry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante SetAsync per {Key}", key);
                return false;
            }
        }

        public async Task<bool> RemoveAsync(string key)
        {
            try
            {
                return await _database.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante RemoveAsync per {Key}", key);
                return false;
            }
        }

        public async Task ClearAllAsync()
        {
            try
            {
                foreach (var endpoint in _connection.GetEndPoints())
                {
                    var server = _connection.GetServer(endpoint);
                    await server.FlushAllDatabasesAsync();
                }
                _logger.LogInformation("Cache svuotata");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante ClearAllAsync");
                throw new RedisConnectionException(ConnectionFailureType.UnableToConnect, "Errore durante lo svuotamento", ex);
            }
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}