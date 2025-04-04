using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using FlexCore.Caching.Core.Interfaces;

namespace FlexCore.Caching.Redis
{
    /// <summary>
    /// Implementazione di un provider di cache basato su Redis
    /// </summary>
    public class RedisCacheProvider : ICacheProvider, IDisposable
    {
        private readonly IConnectionMultiplexer _connection;
        private readonly ILogger<RedisCacheProvider> _logger;
        private readonly IDatabase _database;
        private readonly JsonSerializerOptions _serializerOptions;

        /// <summary>
        /// Inizializza una nuova istanza del provider Redis
        /// </summary>
        /// <param name="connectionString">Stringa di connessione Redis</param>
        /// <param name="logger">Istanza del logger</param>
        /// <param name="connection">Connessione Redis esistente (opzionale)</param>
        /// <param name="serializerOptions">Opzioni di serializzazione JSON (opzionale)</param>
        /// <exception cref="RedisConnectionException">Errore durante la connessione a Redis</exception>
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
                _logger.LogInformation("Connesso a Redis su {Endpoint}", _connection.GetEndPoints().First());
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Connessione fallita a Redis");
                throw new RedisConnectionException(
                    ConnectionFailureType.UnableToConnect,
                    "Errore di connessione a Redis",
                    ex);
            }
        }

        /// <summary>
        /// Verifica l'esistenza di una chiave nella cache
        /// </summary>
        /// <param name="key">Chiave da verificare</param>
        /// <returns>True se la chiave esiste</returns>
        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                return await _database.KeyExistsAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante ExistsAsync per la chiave {Key}", key);
                return false;
            }
        }

        /// <summary>
        /// Ottiene un valore dalla cache
        /// </summary>
        /// <typeparam name="T">Tipo del valore da deserializzare</typeparam>
        /// <param name="key">Chiave associata al valore</param>
        /// <returns>Valore deserializzato o default</returns>
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

                // Ottimizzazione per stringhe
                if (typeof(T) == typeof(string))
                    return (T)(object)redisValue.ToString();

                return JsonSerializer.Deserialize<T>(redisValue.ToString(), _serializerOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante GetAsync per la chiave {Key}", key);
                return default;
            }
        }

        /// <summary>
        /// Imposta un valore nella cache
        /// </summary>
        /// <typeparam name="T">Tipo del valore da serializzare</typeparam>
        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan expiry)
        {
            try
            {
                var serializedValue = JsonSerializer.Serialize(value, _serializerOptions);
                return await _database.StringSetAsync(key, serializedValue, expiry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante SetAsync per la chiave {Key}", key);
                return false;
            }
        }

        /// <summary>
        /// Rimuove una chiave dalla cache
        /// </summary>
        /// <param name="key">Chiave da rimuovere</param>
        public async Task<bool> RemoveAsync(string key)
        {
            try
            {
                return await _database.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante RemoveAsync per la chiave {Key}", key);
                return false;
            }
        }

        /// <summary>
        /// Svuota completamente la cache
        /// </summary>
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
                throw new RedisConnectionException(
                    ConnectionFailureType.UnableToConnect,
                    "Errore durante lo svuotamento",
                    ex);
            }
        }

        /// <summary>
        /// Rilascia le risorse della connessione
        /// </summary>
        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}