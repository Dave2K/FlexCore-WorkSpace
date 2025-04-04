using FlexCore.Caching.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace FlexCore.Caching.Core
{
    /// <summary>
    /// Classe base per la gestione della cache
    /// </summary>
    public abstract class BaseCacheManager
    {
        protected readonly ILogger _logger;
        protected readonly ICacheProvider _cacheProvider;

        /// <summary>
        /// Inizializza una nuova istanza del gestore cache
        /// </summary>
        protected BaseCacheManager(ILogger logger, ICacheProvider cacheProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
        }

        /// <summary>
        /// Verifica l'esistenza di una chiave nella cache
        /// </summary>
        public virtual async Task<bool> ExistsAsync(string key)
        {
            ValidateKey(key);
            return await _cacheProvider.ExistsAsync(key);
        }

        /// <summary>
        /// Valida il formato delle chiavi di cache
        /// </summary>
        protected virtual void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Chiave non valida", nameof(key));
        }
    }
}