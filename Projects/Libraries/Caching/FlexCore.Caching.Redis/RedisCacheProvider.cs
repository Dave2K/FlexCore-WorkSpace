using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System;
using FlexCore.Caching.Core.Interfaces;

namespace FlexCore.Caching.Redis
{
    public class RedisCacheProvider : ICacheProvider
    {
        private readonly IDatabase _redisDb;
        private readonly ILogger<RedisCacheProvider> _logger;
        private readonly IConnectionMultiplexer _connection;

        public RedisCacheProvider(
            IConnectionMultiplexer connection,
            ILogger<RedisCacheProvider> logger)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _redisDb = connection.GetDatabase();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool Exists(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key non valida", nameof(key));

            return _redisDb.KeyExists(key);
        }

        public T? Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key non valida", nameof(key));

            var value = _redisDb.StringGet(key);
            return value.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(value!);
        }

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

        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key non valida", nameof(key));

            _redisDb.KeyDelete(key);
            _logger.LogInformation($"Rimossa chiave Redis: {key}");
        }

        public void ClearAll()
        {
            try
            {
                var endpoints = _connection.GetEndPoints();
                if (endpoints == null || endpoints.Length == 0) return;

                var server = _connection.GetServer(endpoints[0]);
                if (server == null) return;

                foreach (var key in server.Keys(pattern: "*"))
                {
                    _redisDb.KeyDelete(key);
                }
                _logger.LogInformation("Cache Redis completamente svuotata");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante lo svuotamento della cache Redis");
                throw;
            }
        }
    }
}