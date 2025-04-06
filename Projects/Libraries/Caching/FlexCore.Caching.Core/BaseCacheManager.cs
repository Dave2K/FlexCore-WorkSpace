using FlexCore.Caching.Common.Validators;
using FlexCore.Caching.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FlexCore.Caching.Core
{
    /// <summary>  
    /// Fornisce funzionalità base per la gestione della cache con validazione centralizzata.  
    /// </summary>  
    public abstract class BaseCacheManager
    {
        protected readonly ILogger _logger;
        protected readonly ICacheProvider _cacheProvider;

        /// <summary>  
        /// Inizializza una nuova istanza del gestore di cache.  
        /// </summary>  
        protected BaseCacheManager(ILogger logger, ICacheProvider cacheProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
            _logger.LogDebug("BaseCacheManager inizializzato per {ProviderType}", cacheProvider.GetType().Name);
        }

        /// <summary>  
        /// Verifica l'esistenza di una chiave nella cache.  
        /// </summary>  
        public virtual async Task<bool> ExistsAsync(string key)
        {
            CacheKeyValidator.ValidateKey(key);

            try
            {
                _logger.LogDebug("Verifica esistenza chiave: {Key}", key);
                return await _cacheProvider.ExistsAsync(key).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Verifica fallita per {Key}", key);
                throw;
            }
        }

        /// <summary>  
        /// Ottiene un valore dalla cache.  
        /// </summary>  
        public virtual async Task<T?> GetAsync<T>(string key)
        {
            CacheKeyValidator.ValidateKey(key);

            try
            {
                _logger.LogDebug("Recupero chiave: {Key}", key);
                return await _cacheProvider.GetAsync<T>(key).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Recupero fallito per {Key}", key);
                return default;
            }
        }

        /// <summary>  
        /// Memorizza un valore nella cache con scadenza.  
        /// </summary>  
        public virtual async Task<bool> SetAsync<T>(string key, T value, TimeSpan expiry)
        {
            CacheKeyValidator.ValidateKey(key);

            try
            {
                _logger.LogDebug("Salvataggio chiave: {Key}", key);
                return await _cacheProvider.SetAsync(key, value, expiry).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Salvataggio fallito per {Key}", key);
                return false;
            }
        }

        /// <summary>  
        /// Rimuove una chiave dalla cache.  
        /// </summary>  
        public virtual async Task<bool> RemoveAsync(string key)
        {
            CacheKeyValidator.ValidateKey(key);

            try
            {
                _logger.LogDebug("Rimozione chiave: {Key}", key);
                return await _cacheProvider.RemoveAsync(key).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rimozione fallita per {Key}", key);
                return false;
            }
        }

        /// <summary>  
        /// Svuota completamente la cache.  
        /// </summary>  
        public virtual async Task ClearAllAsync()
        {
            try
            {
                _logger.LogDebug("Svuotamento cache");
                await _cacheProvider.ClearAllAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Svuotamento cache fallito");
                throw;
            }
        }
    }
}