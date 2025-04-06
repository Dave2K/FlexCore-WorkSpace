using FlexCore.Caching.Common.Validators;
using FlexCore.Caching.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FlexCore.Caching.Core
{
    public abstract class BaseCacheManager
    {
        protected readonly ILogger _logger;
        protected readonly ICacheProvider _cacheProvider;

        protected BaseCacheManager(ILogger logger, ICacheProvider cacheProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
        }

        public virtual async Task<bool> ExistsAsync(string key)
        {
            CacheKeyValidator.ThrowIfInvalid(key); // ✅ Sostituito ValidateKey
            return await _cacheProvider.ExistsAsync(key).ConfigureAwait(false);
        }

        public virtual async Task<T?> GetAsync<T>(string key)
        {
            CacheKeyValidator.ThrowIfInvalid(key); // ✅ Sostituito ValidateKey
            return await _cacheProvider.GetAsync<T>(key).ConfigureAwait(false);
        }

        public virtual async Task<bool> SetAsync<T>(string key, T value, TimeSpan expiry)
        {
            CacheKeyValidator.ThrowIfInvalid(key); // ✅ Sostituito ValidateKey
            return await _cacheProvider.SetAsync(key, value, expiry).ConfigureAwait(false);
        }

        public virtual async Task<bool> RemoveAsync(string key)
        {
            CacheKeyValidator.ThrowIfInvalid(key); // ✅ Sostituito ValidateKey
            return await _cacheProvider.RemoveAsync(key).ConfigureAwait(false);
        }

        public virtual async Task ClearAllAsync()
        {
            await _cacheProvider.ClearAllAsync().ConfigureAwait(false);
        }
    }
}