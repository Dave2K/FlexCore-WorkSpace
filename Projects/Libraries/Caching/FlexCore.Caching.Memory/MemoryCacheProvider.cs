using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using FlexCore.Caching.Core.Interfaces;

namespace FlexCore.Caching.Memory
{
    /// <summary>
    /// Provider di cache in-memory
    /// </summary>
    public sealed class MemoryCacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<MemoryCacheProvider> _logger;

        public MemoryCacheProvider(
            IMemoryCache cache,
            ILogger<MemoryCacheProvider> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public bool Exists(string key) => _cache.TryGetValue(key, out _);

        public T? Get<T>(string key)
        {
            return _cache.TryGetValue(key, out T? value) ? value : default;
        }

        public void Set<T>(string key, T value, TimeSpan expiry)
        {
            _cache.Set(key, value, expiry);
            _logger.LogInformation($"Impostato valore per la chiave: {key}");
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
            _logger.LogInformation($"Rimossa chiave: {key}");
        }

        public void ClearAll()
        {
            if (_cache is MemoryCache memCache)
            {
                memCache.Compact(1.0);
                _logger.LogInformation("Cache svuotata completamente");
            }
        }
    }
}