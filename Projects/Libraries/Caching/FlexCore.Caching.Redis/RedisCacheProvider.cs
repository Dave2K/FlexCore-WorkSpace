using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System;
using FlexCore.Caching.Core.Interfaces;

namespace FlexCore.Caching.Redis
{
    /// <summary>
    /// Provider di cache basato su Redis per le operazioni di memorizzazione e recupero dati.
    /// </summary>
    public class RedisCacheProvider : ICacheProvider
    {
        private readonly IDatabase _redisDb;
        private readonly ILogger<RedisCacheProvider> _logger;

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="RedisCacheProvider"/>.
        /// </summary>
        /// <param name="connection">Connessione a Redis.</param>
        /// <param name="logger">Logger per la tracciatura delle operazioni.</param>
        /// <exception cref="ArgumentNullException">Se <paramref name="connection"/> o <paramref name="logger"/> sono null.</exception>
        public RedisCacheProvider(
            IConnectionMultiplexer connection,
            ILogger<RedisCacheProvider> logger)
        {
            _redisDb = connection?.GetDatabase()
                ?? throw new ArgumentNullException(nameof(connection));
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Verifica se una chiave esiste nella cache Redis.
        /// </summary>
        /// <param name="key">Chiave da verificare.</param>
        /// <returns>True se la chiave esiste, altrimenti False.</returns>
        /// <exception cref="ArgumentException">Se <paramref name="key"/> è nullo o vuoto.</exception>
        public bool Exists(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key non valida", nameof(key));

            return _redisDb.KeyExists(key);
        }

        /// <summary>
        /// Recupera un valore dalla cache Redis.
        /// </summary>
        /// <typeparam name="T">Tipo del valore da recuperare.</typeparam>
        /// <param name="key">Chiave associata al valore.</param>
        /// <returns>Il valore deserializzato o default(T) se non presente.</returns>
        /// <exception cref="ArgumentException">Se <paramref name="key"/> è nullo o vuoto.</exception>
        public T? Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key non valida", nameof(key));

            var value = _redisDb.StringGet(key);
            return value.IsNullOrEmpty
                ? default
                : JsonSerializer.Deserialize<T>(value!);
        }

        /// <summary>
        /// Imposta un valore nella cache Redis con una scadenza specificata.
        /// </summary>
        /// <typeparam name="T">Tipo del valore da memorizzare.</typeparam>
        /// <param name="key">Chiave associata al valore.</param>
        /// <param name="value">Valore da memorizzare.</param>
        /// <param name="expiry">Durata di validità della cache.</param>
        /// <exception cref="ArgumentException">Se <paramref name="key"/> è nullo o vuoto.</exception>
        public void Set<T>(string key, T value, TimeSpan expiry)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key non valida", nameof(key));

            _redisDb.StringSet(
                key,
                JsonSerializer.Serialize(value),
                expiry
            );
            _logger.LogInformation($"Impostato valore in Redis per la chiave: {key}");
        }

        /// <summary>
        /// Rimuove una chiave dalla cache Redis.
        /// </summary>
        /// <param name="key">Chiave da rimuovere.</param>
        /// <exception cref="ArgumentException">Se <paramref name="key"/> è nullo o vuoto.</exception>
        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key non valida", nameof(key));

            _redisDb.KeyDelete(key);
            _logger.LogInformation($"Rimossa chiave Redis: {key}");
        }

        /// <summary>
        /// Svuota completamente la cache Redis eliminando tutte le chiavi.
        /// </summary>
        /// <remarks>
        /// Logga un messaggio informativo dopo l'operazione.
        /// </remarks>
        public void ClearAll()
        {
            var endpoints = _redisDb.Multiplexer.GetEndPoints();
            if (endpoints == null || endpoints.Length == 0) return;

            var server = _redisDb.Multiplexer.GetServer(endpoints[0]);
            if (server == null) return;

            foreach (var key in server.Keys(pattern: "*"))
            {
                _redisDb.KeyDelete(key);
            }
            _logger.LogInformation("Cache Redis completamente svuotata");
        }
    }
}