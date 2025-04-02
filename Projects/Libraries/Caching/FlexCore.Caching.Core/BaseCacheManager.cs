using Microsoft.Extensions.Logging;
using FlexCore.Caching.Core.Interfaces;

namespace FlexCore.Caching.Core
{
    /// <summary>
    /// Classe astratta base per la gestione della cache
    /// </summary>
    public abstract class BaseCacheManager
    {
        protected readonly ILogger _logger;
        protected readonly ICacheProvider _cacheProvider;

        protected BaseCacheManager(
            ILogger logger,
            ICacheProvider cacheProvider)
        {
            _logger = logger;
            _cacheProvider = cacheProvider;
        }

        public virtual bool Exists(string key) => _cacheProvider.Exists(key);
        public virtual T? Get<T>(string key) => _cacheProvider.Get<T>(key);
        public virtual void Set<T>(string key, T value, TimeSpan expiry)
            => _cacheProvider.Set(key, value, expiry);
        public virtual void Remove(string key) => _cacheProvider.Remove(key);
        public virtual void ClearAll() => _cacheProvider.ClearAll();
    }
}