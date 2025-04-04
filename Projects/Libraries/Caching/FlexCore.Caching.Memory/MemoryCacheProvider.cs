using FlexCore.Caching.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FlexCore.Caching.Memory
{
    /// <summary>
    /// Implementazione di un provider di cache in memoria
    /// </summary>
    public sealed class MemoryCacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<MemoryCacheProvider> _logger;

        /// <summary>
        /// Inizializza una nuova istanza del provider
        /// </summary>
        /// <param name="cache">Istanza di MemoryCache</param>
        /// <param name="logger">Logger per tracciamento attività</param>
        /// <exception cref="ArgumentNullException">
        /// Se <paramref name="cache"/> o <paramref name="logger"/> sono null
        /// </exception>
        public MemoryCacheProvider(
            IMemoryCache cache,
            ILogger<MemoryCacheProvider> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        /// <inheritdoc/>
        public Task ClearAllAsync()
        {
            try
            {
                if (_cache is MemoryCache memoryCache)
                {
                    memoryCache.Compact(1.0);
                    _logger.LogInformation("Cache completamente svuotata");
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante lo svuotamento della cache");
                throw; // Rilancia per permettere gestione centralizzata
            }
        }

        /// <inheritdoc/>
        public Task<bool> ExistsAsync(string key)
        {
            return Task.FromResult(_cache.TryGetValue(key, out _));
        }

        /// <inheritdoc/>
        public Task<T?> GetAsync<T>(string key)
        {
            return Task.FromResult(_cache.Get<T>(key));
        }

        /// <inheritdoc/>
        public Task<bool> RemoveAsync(string key)
        {
            try
            {
                _cache.Remove(key);
                _logger.LogDebug("Chiave {Key} rimossa", key);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la rimozione della chiave {Key}", key);
                return Task.FromResult(false);
            }
        }

        /// <inheritdoc/>
        public Task<bool> SetAsync<T>(string key, T value, TimeSpan expiry)
        {
            try
            {
                _cache.Set(key, value, expiry);
                _logger.LogDebug("Chiave {Key} salvata", key);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il salvataggio della chiave {Key}", key);
                return Task.FromResult(false);
            }
        }
    }
}