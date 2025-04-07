using FlexCore.Caching.Core.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlexCore.Caching.Redis
{
    /// <summary>
    /// Implementazione di un provider Redis per la gestione avanzata della cache
    /// </summary>
    /// <remarks>
    /// Fornisce:
    /// - Serializzazione/deserializzazione JSON
    /// - Gestione centralizzata degli errori
    /// - Logging dettagliato
    /// - Connessioni multiplexate
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
        /// <param name="connectionString">Stringa di connessione Redis formattata (es: "host:port")</param>
        /// <param name="logger">Istanza del logger per tracciamento attività</param>
        /// <param name="connection">Connessione esistente (opzionale per testing)</param>
        /// <param name="serializerOptions">Opzioni personalizzate per la serializzazione JSON</param>
        /// <exception cref="ArgumentNullException">
        /// Generato se <paramref name="connectionString"/> o <paramref name="logger"/> sono null/vuoti
        /// </exception>
        public RedisCacheProvider(
            string connectionString,
            ILogger<RedisCacheProvider> logger,
            IConnectionMultiplexer? connection = null,
            JsonSerializerOptions? serializerOptions = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connection = connection ?? ConnectionMultiplexer.Connect(connectionString);
            _database = _connection.GetDatabase();
            _serializerOptions = serializerOptions ?? new JsonSerializerOptions();
        }

        /// <inheritdoc/>
        public async Task<T?> GetAsync<T>(string key)
        {
            ValidateKey(key);
            try
            {
                var redisValue = await _database.StringGetAsync(key).ConfigureAwait(false);

                if (redisValue.IsNullOrEmpty)
                {
                    _logger.LogDebug("Chiave {Key} non presente in cache", key);
                    return default;
                }

                return JsonSerializer.Deserialize<T>(redisValue.ToString(), _serializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError(
                    ex,
                    "Errore deserializzazione JSON per la chiave {Key}. Dati: {RawData}",
                    key,
                    ex.Data.Count > 0 ? ex.Data["Json"] : "N/A"
                );
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Errore critico durante l'accesso alla chiave {Key}",
                    key
                );
                throw new RedisCacheException($"Operazione fallita per la chiave {key}", ex);
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
            catch (JsonException ex)
            {
                _logger.LogError(
                    ex,
                    "Errore serializzazione JSON per la chiave {Key}",
                    key
                );
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveAsync(string key)
        {
            ValidateKey(key);
            return await _database.KeyDeleteAsync(key);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(string key)
        {
            ValidateKey(key);
            return await _database.KeyExistsAsync(key);
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

        /// <summary>
        /// Valida il formato della chiave secondo le policy aziendali
        /// </summary>
        /// <param name="key">Chiave da validare</param>
        /// <exception cref="ArgumentException">
        /// Generato se la chiave non rispetta i requisiti di formato
        /// </exception>
        private static void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("La chiave non può essere vuota o contenere solo spazi", nameof(key));

            if (key.Length > 128)
                throw new ArgumentException("La chiave non può superare 128 caratteri", nameof(key));
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}