using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using FlexCore.Caching.Core.Interfaces;

namespace FlexCore.Caching.Memory
{
    /// <summary>
    /// Provider di cache in memoria con supporto asincrono
    /// </summary>
    /// <remarks>
    /// Implementazione basata su IMemoryCache di Microsoft.Extensions.Caching.Memory
    /// </remarks>
    public sealed class MemoryCacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<MemoryCacheProvider> _logger;

        /// <summary>
        /// Inizializza una nuova istanza del provider
        /// </summary>
        /// <param name="cache">Istanza della cache</param>
        /// <param name="logger">Logger per tracciamento</param>
        /// <exception cref="ArgumentNullException">Se parametri sono null</exception>
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
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Chiave non valida", nameof(key));

            return Task.FromResult(_cache.TryGetValue(key, out _));
        }

        /// <inheritdoc/>
        public Task<T?> GetAsync<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Chiave non valida", nameof(key));

            return Task.FromResult(_cache.Get<T>(key));
        }

        /// <inheritdoc/>
        public Task<bool> SetAsync<T>(string key, T value, TimeSpan expiry)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Chiave non valida", nameof(key));

            try
            {
                _cache.Set(key, value, expiry);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il salvataggio in memoria");
                return Task.FromResult(false);
            }
        }

        /// <inheritdoc/>
        public Task<bool> RemoveAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Chiave non valida", nameof(key));

            _cache.Remove(key);
            return Task.FromResult(true);
        }

        /// <inheritdoc/>
        public Task ClearAllAsync()
        {
            if (_cache is MemoryCache memoryCache)
            {
                memoryCache.Compact(1.0);
                _logger.LogInformation("Cache memoria svuotata");
            }
            return Task.CompletedTask;
        }
    }
}