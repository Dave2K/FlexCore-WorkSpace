using System;
using System.Threading.Tasks;
using FlexCore.Caching.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlexCore.Caching.Core
{
    /// <summary>
    /// Classe base astratta per la gestione asincrona della cache.
    /// </summary>
    public abstract class BaseCacheManager
    {
        protected readonly ILogger _logger;
        protected readonly ICacheProvider _cacheProvider;

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="BaseCacheManager"/>.
        /// </summary>
        /// <param name="logger">Il logger per la registrazione delle operazioni.</param>
        /// <param name="cacheProvider">Il provider di cache da utilizzare.</param>
        protected BaseCacheManager(ILogger logger, ICacheProvider cacheProvider)
        {
            _logger = logger;
            _cacheProvider = cacheProvider;
        }

        /// <summary>
        /// Verifica in modo asincrono se una chiave esiste nella cache.
        /// </summary>
        /// <param name="key">La chiave da verificare.</param>
        /// <returns>
        /// Un Task che restituisce True se la chiave esiste, altrimenti False.
        /// </returns>
        public virtual async Task<bool> ExistsAsync(string key)
        {
            return await _cacheProvider.ExistsAsync(key);
        }

        /// <summary>
        /// Ottiene in modo asincrono un valore dalla cache.
        /// </summary>
        /// <typeparam name="T">Il tipo del valore da ottenere.</typeparam>
        /// <param name="key">La chiave associata al valore.</param>
        /// <returns>
        /// Un Task che restituisce il valore associato alla chiave, oppure il valore di default se non presente.
        /// </returns>
        public virtual async Task<T?> GetAsync<T>(string key)
        {
            return await _cacheProvider.GetAsync<T>(key);
        }

        /// <summary>
        /// Imposta in modo asincrono un valore nella cache.
        /// </summary>
        /// <typeparam name="T">Il tipo del valore da impostare.</typeparam>
        /// <param name="key">La chiave per il valore.</param>
        /// <param name="value">Il valore da impostare.</param>
        /// <param name="expiry">La durata dopo la quale il valore scade.</param>
        /// <returns>
        /// Un Task che rappresenta l'operazione asincrona di impostazione.
        /// </returns>
        public virtual async Task SetAsync<T>(string key, T value, TimeSpan expiry)
        {
            await _cacheProvider.SetAsync(key, value, expiry);
        }

        /// <summary>
        /// Rimuove in modo asincrono un valore dalla cache.
        /// </summary>
        /// <param name="key">La chiave del valore da rimuovere.</param>
        /// <returns>
        /// Un Task che rappresenta l'operazione asincrona di rimozione.
        /// </returns>
        public virtual async Task RemoveAsync(string key)
        {
            await _cacheProvider.RemoveAsync(key);
        }

        /// <summary>
        /// Svuota in modo asincrono tutti i valori presenti nella cache.
        /// </summary>
        /// <returns>
        /// Un Task che rappresenta l'operazione asincrona di svuotamento della cache.
        /// </returns>
        public virtual async Task ClearAllAsync()
        {
            await _cacheProvider.ClearAllAsync();
        }
    }
}
