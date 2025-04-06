using FlexCore.Caching.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FlexCore.Caching.Memory
{
    public sealed class MemoryCacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<MemoryCacheProvider> _logger;

        public MemoryCacheProvider(IMemoryCache cache, ILogger<MemoryCacheProvider> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<bool> ExistsAsync(string key)
        {
            return Task.FromResult(_cache.TryGetValue(key, out _));
        }

        public Task<T?> GetAsync<T>(string key)
        {
            return Task.FromResult(_cache.Get<T>(key));
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan expiry)
        {
            try
            {
                await Task.Run(() => _cache.Set(key, value, expiry));
                _logger.LogDebug("Chiave {Key} salvata", key);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il salvataggio di {Key}", key);
                return false;
            }
        }

        public async Task<bool> RemoveAsync(string key)
        {
            try
            {
                await Task.Run(() => _cache.Remove(key));
                _logger.LogDebug("Chiave {Key} rimossa", key);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la rimozione di {Key}", key);
                return false;
            }
        }

        public Task ClearAllAsync()
        {
            if (_cache is MemoryCache memoryCache)
            {
                memoryCache.Compact(1.0);
                _logger.LogInformation("Cache svuotata");
            }
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_cache is MemoryCache memoryCache)
                memoryCache.Dispose();
        }
    }
}