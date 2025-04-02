using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using FlexCore.Caching.Core.Interfaces;
using System.Text.Json;

namespace FlexCore.Caching.Redis;

/// <summary>
/// Implementazione di ICacheProvider che utilizza Redis
/// </summary>
/// <param name="connection">Connessione a Redis</param>
/// <param name="logger">Logger per la tracciatura delle operazioni</param>
public class RedisCacheProvider(IConnectionMultiplexer connection, ILogger<RedisCacheProvider> logger)
    : ICacheProvider
{
    private readonly IDatabase _redisDb = connection.GetDatabase();
    private readonly ILogger<RedisCacheProvider> _logger = logger;

    public bool Exists(string key) => _redisDb.KeyExists(key);

    public T? Get<T>(string key)
    {
        var value = _redisDb.StringGet(key);
        return value.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(value!);
    }

    public void Set<T>(string key, T value, TimeSpan expiry)
        => _redisDb.StringSet(key, JsonSerializer.Serialize(value), expiry);

    public void Remove(string key) => _redisDb.KeyDelete(key);

    public void ClearAll()
    {
        var endpoints = _redisDb.Multiplexer.GetEndPoints();
        var server = _redisDb.Multiplexer.GetServer(endpoints.First());
        foreach (var key in server.Keys(pattern: "*"))
        {
            _redisDb.KeyDelete(key);
        }
    }
}