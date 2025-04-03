using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace FlexCore.Caching.Redis
{
    /// <summary>
    /// Fornisce un'implementazione thread-safe per l'interazione con Redis
    /// </summary>
    /// <remarks>
    /// Implementa il pattern IDisposable per la gestione corretta delle risorse.
    /// Supporta operazioni CRUD, Pub/Sub e gestione avanzata delle connessioni.
    /// </remarks>
    public sealed class RedisCacheProvider : IDisposable
    {
        private readonly IConnectionMultiplexer _connection;
        private readonly ILogger<RedisCacheProvider> _logger;
        private bool _disposed;

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="RedisCacheProvider"/>
        /// </summary>
        /// <param name="connectionString">Stringa di connessione Redis nel formato 'host:port'</param>
        /// <param name="logger">Istanza del logger per il tracciamento delle attività</param>
        /// <param name="connection">Istanza opzionale di connessione (usata principalmente per i test)</param>
        /// <exception cref="ArgumentNullException">Se <paramref name="connectionString"/> o <paramref name="logger"/> sono null</exception>
        public RedisCacheProvider(
            string connectionString,
            ILogger<RedisCacheProvider> logger,
            IConnectionMultiplexer? connection = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var config = new ConfigurationOptions
            {
                EndPoints = { connectionString },
                AbortOnConnectFail = false,
                ConnectRetry = 3,
                ConnectTimeout = 5000
            };

            _connection = connection ?? ConnectionMultiplexer.Connect(config);

            _connection.ConnectionFailed += (_, e) =>
                _logger.LogError(e.Exception, "Connessione Redis interrotta: {FailureType}", e.FailureType);

            _connection.ConnectionRestored += (_, _) =>
                _logger.LogInformation("Connessione Redis ripristinata");
        }

        /// <summary>
        /// Ottiene il database Redis predefinito (db 0)
        /// </summary>
        public IDatabase Database => _connection.GetDatabase();

        /// <summary>
        /// Verifica se la connessione a Redis è attiva
        /// </summary>
        public bool IsConnected => _connection.IsConnected;

        /// <summary>
        /// Verifica in modo asincrono l'esistenza di una chiave
        /// </summary>
        /// <param name="key">Chiave da verificare</param>
        /// <returns>Task che restituisce true se la chiave esiste</returns>
        /// <exception cref="ArgumentException">Se <paramref name="key"/> è null o vuoto</exception>
        public async Task<bool> ExistsAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Chiave non valida", nameof(key));

            return await Database.KeyExistsAsync(key).ConfigureAwait(false);
        }

        /// <summary>
        /// Ottiene in modo asincrono il valore associato a una chiave
        /// </summary>
        /// <typeparam name="T">Tipo dell'oggetto da deserializzare</typeparam>
        /// <param name="key">Chiave da cui recuperare il valore</param>
        /// <returns>Valore deserializzato o default se non trovato</returns>
        /// <exception cref="ArgumentException">Se <paramref name="key"/> è null o vuoto</exception>
        public async Task<T?> GetAsync<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Chiave non valida", nameof(key));

            try
            {
                var value = await Database.StringGetAsync(key).ConfigureAwait(false);
                return value.HasValue
                    ? JsonSerializer.Deserialize<T>(value.ToString())
                    : default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero della chiave {Key}", key);
                return default;
            }
        }

        /// <summary>
        /// Imposta in modo asincrono un valore con scadenza
        /// </summary>
        /// <typeparam name="T">Tipo dell'oggetto da serializzare</typeparam>
        /// <param name="key">Chiave da impostare</param>
        /// <param name="value">Valore da memorizzare</param>
        /// <param name="expiry">Durata di validità</param>
        /// <returns>True se l'operazione è riuscita</returns>
        /// <exception cref="ArgumentException">Se <paramref name="key"/> è null o vuoto</exception>
        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan expiry)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Chiave non valida", nameof(key));

            try
            {
                var serializedValue = JsonSerializer.Serialize(value);
                return await Database.StringSetAsync(
                    key,
                    serializedValue,
                    expiry,
                    When.Always,
                    CommandFlags.None
                ).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il salvataggio della chiave {Key}", key);
                return false;
            }
        }

        /// <summary>
        /// Rimuove in modo asincrono una chiave
        /// </summary>
        /// <param name="key">Chiave da rimuovere</param>
        /// <returns>True se la chiave è stata rimossa</returns>
        /// <exception cref="ArgumentException">Se <paramref name="key"/> è null o vuoto</exception>
        public async Task<bool> RemoveAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Chiave non valida", nameof(key));

            return await Database.KeyDeleteAsync(key).ConfigureAwait(false);
        }

        /// <summary>
        /// Pubblica un messaggio su un canale Pub/Sub
        /// </summary>
        /// <param name="channel">Nome del canale</param>
        /// <param name="message">Messaggio da pubblicare</param>
        /// <returns>Numero di client che hanno ricevuto il messaggio</returns>
        public async Task<long> PublishAsync(string channel, string message)
        {
            var sub = _connection.GetSubscriber();
            return await sub.PublishAsync(
                RedisChannel.Literal(channel),
                message,
                CommandFlags.None
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// Sottoscrive un canale Pub/Sub
        /// </summary>
        /// <param name="channel">Nome del canale</param>
        /// <param name="messageHandler">Handler per i messaggi ricevuti</param>
        public void Subscribe(string channel, Action<string, string?> messageHandler)
        {
            var sub = _connection.GetSubscriber();
            sub.Subscribe(
                RedisChannel.Literal(channel),
                (_, value) => messageHandler(channel, value.ToString())
            );
        }

        /// <summary>
        /// Svuota tutti i database Redis
        /// </summary>
        public async Task FlushAllAsync()
        {
            var endpoints = _connection.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = _connection.GetServer(endpoint);
                await server.FlushAllDatabasesAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Rilascia le risorse gestite dalla connessione
        /// </summary>
        /// <remarks>
        /// Metodo idempotente: può essere chiamato multipli volte senza effetti collaterali
        /// </remarks>
        public void Dispose()
        {
            if (_disposed) return;

            _connection.Close();
            _connection.Dispose();
            _disposed = true;
        }
    }
}