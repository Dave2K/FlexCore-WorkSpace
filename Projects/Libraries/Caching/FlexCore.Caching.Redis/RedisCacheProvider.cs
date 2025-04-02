using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using FlexCore.Caching.Core.Interfaces;

namespace FlexCore.Caching.Redis
{
    /// <summary>
    /// Provider di cache Redis
    /// </summary>
    public class RedisCacheProvider(
        IConnectionMultiplexer connection,
        ILogger<RedisCacheProvider> logger) : ICacheProvider
    {
        private readonly IDatabase _redisDb = connection.GetDatabase();
        private readonly ILogger<RedisCacheProvider> _logger = logger;

        public bool Exists(string key) => _redisDb.KeyExists(key);

        public T? Get<T>(string key)
        {
            var value = _redisDb.StringGet(key);
            return value.HasValue ?
                System.Text.Json.JsonSerializer.Deserialize<T>(value!) :
                default;
        }

        public void Set<T>(string key, T value, TimeSpan expiry)
        {
            _redisDb.StringSet(
                key,
                System.Text.Json.JsonSerializer.Serialize(value),
                expiry
            );
            _logger.LogInformation($"Impostato valore Redis per la chiave: {key}");
        }

        public void Remove(string key)
        {
            _redisDb.KeyDelete(key);
            _logger.LogInformation($"Rimossa chiave Redis: {key}");
        }

        public void ClearAll()
        {
            // Redis non supporta ClearAll, implementazione vuota
            _logger.LogWarning("Redis non supporta l'operazione ClearAll");
        }
    }
}