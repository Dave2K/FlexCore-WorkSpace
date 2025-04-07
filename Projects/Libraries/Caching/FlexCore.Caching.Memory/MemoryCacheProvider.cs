using FlexCore.Caching.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FlexCore.Caching.Memory
{
    /// <summary>
    /// Implementazione di ICacheProvider utilizzando MemoryCache
    /// </summary>
    public sealed class MemoryCacheProvider : ICacheProvider, IDisposable
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<MemoryCacheProvider> _logger;

        /// <summary>
        /// Inizializza una nuova istanza del provider
        /// </summary>
        /// <param name="cache">Istanza di IMemoryCache</param>
        /// <param name="logger">Logger per tracciamento</param>
        public MemoryCacheProvider(
            IMemoryCache cache,
            ILogger<MemoryCacheProvider> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public Task<bool> ExistsAsync(string key)
        {
            ValidateKey(key);
            return Task.FromResult(_cache.TryGetValue(key, out _));
        }

        /// <inheritdoc/>
        public Task<T?> GetAsync<T>(string key)
        {
            ValidateKey(key);
            return Task.FromResult(_cache.Get<T>(key));
        }

        /// <inheritdoc/>
        public Task<bool> SetAsync<T>(string key, T value, TimeSpan expiry)
        {
            ValidateKey(key);
            _cache.Set(key, value, expiry);
            return Task.FromResult(true);
        }

        /// <inheritdoc/>
        public Task<bool> RemoveAsync(string key)
        {
            ValidateKey(key);
            _cache.Remove(key);
            return Task.FromResult(true);
        }

        /// <inheritdoc/>
        public Task ClearAllAsync()
        {
            if (_cache is MemoryCache memoryCache)
                memoryCache.Compact(1.0);
            return Task.CompletedTask;
        }

        private static void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Chiave non valida", nameof(key));
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_cache is MemoryCache memoryCache)
                memoryCache.Dispose();
        }
    }
}