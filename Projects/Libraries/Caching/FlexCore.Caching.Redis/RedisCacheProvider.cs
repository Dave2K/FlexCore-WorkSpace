using FlexCore.Caching.Core.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlexCore.Caching.Redis
{
    /// <summary>
    /// Implementazione di ICacheProvider utilizzando Redis
    /// </summary>
    /// <remarks>
    /// Gestisce la serializzazione/deserializzazione JSON e la comunicazione con Redis
    /// </remarks>
    public sealed class RedisCacheProvider : ICacheProvider, IDisposable
    {
        private readonly IConnectionMultiplexer _connection;
        private readonly IDatabase _database;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<RedisCacheProvider> _logger;

        /// <summary>
        /// Inizializza una nuova istanza del provider Redis
        /// </summary>
        /// <param name="connectionString">Stringa di connessione Redis</param>
        /// <param name="logger">Logger per tracciamento</param>
        /// <param name="connection">Istanza esistente di ConnectionMultiplexer (opzionale)</param>
        /// <param name="serializerOptions">Opzioni di serializzazione JSON (opzionale)</param>
        /// <exception cref="ArgumentNullException">Se connectionString o logger sono null</exception>
        public RedisCacheProvider(
            string connectionString,
            ILogger<RedisCacheProvider> logger,
            IConnectionMultiplexer? connection = null,
            JsonSerializerOptions? serializerOptions = null)
        {
            _connection = connection ?? ConnectionMultiplexer.Connect(connectionString);
            _database = _connection.GetDatabase();
            _serializerOptions = serializerOptions ?? new JsonSerializerOptions();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(string key)
        {
            ValidateKey(key);
            return await _database.KeyExistsAsync(key);
        }

        /// <inheritdoc/>
        public async Task<T?> GetAsync<T>(string key)
        {
            ValidateKey(key);

            try
            {
                var redisValue = await _database.StringGetAsync(key);
                if (redisValue.IsNullOrEmpty)
                {
                    return default;
                }

                var jsonString = redisValue.ToString();

                try
                {
                    return JsonSerializer.Deserialize<T>(jsonString, _serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError(
                        ex,
                        "Deserializzazione fallita per la chiave {Key}. Dati: {JsonData}",
                        key,
                        jsonString);
                    return default;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Errore durante il recupero della chiave {Key}",
                    key);
                throw new RedisCacheException(
                    $"Errore durante l'operazione GET per la chiave {key}",
                    ex);
            }
        }

        /// <inheritdoc/>
        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan expiry)
        {
            ValidateKey(key);

            try
            {
                var serializedValue = JsonSerializer.Serialize(value, _serializerOptions);
                return await _database.StringSetAsync(key, serializedValue, expiry);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Errore durante il salvataggio della chiave {Key}",
                    key);
                throw new RedisCacheException(
                    $"Errore durante l'operazione SET per la chiave {key}",
                    ex);
            }
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveAsync(string key)
        {
            ValidateKey(key);
            return await _database.KeyDeleteAsync(key);
        }

        /// <inheritdoc/>
        public async Task ClearAllAsync()
        {
            foreach (var endpoint in _connection.GetEndPoints())
            {
                var server = _connection.GetServer(endpoint);
                await server.FlushAllDatabasesAsync();
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
            GC.SuppressFinalize(this);
        }

        private static void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Chiave non valida", nameof(key));
            }
        }
    }
}