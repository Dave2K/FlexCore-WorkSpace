using System;
using System.Threading.Tasks;

namespace FlexCore.Caching.Core.Interfaces
{
    /// <summary>
    /// Definisce il contratto per un servizio di cache con operazioni sincrone e asincrone
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Ottiene un valore dalla cache
        /// </summary>
        /// <typeparam name="T">Tipo del valore</typeparam>
        /// <param name="key">Chiave di cache</param>
        T? Get<T>(string key);

        /// <summary>
        /// Memorizza un valore nella cache
        /// </summary>
        /// <typeparam name="T">Tipo del valore</typeparam>
        /// <param name="key">Chiave di cache</param>
        /// <param name="value">Valore da memorizzare</param>
        /// <param name="expiration">Durata di validità</param>
        void Set<T>(string key, T value, TimeSpan expiration);

        /// <summary>
        /// Rimuove una chiave dalla cache
        /// </summary>
        /// <param name="key">Chiave da rimuovere</param>
        bool Remove(string key);

        /// <summary>
        /// Verifica l'esistenza di una chiave
        /// </summary>
        /// <param name="key">Chiave da verificare</param>
        bool Exists(string key);

        /// <summary>
        /// Ottiene un valore dalla cache (asincrono)
        /// </summary>
        /// <typeparam name="T">Tipo del valore</typeparam>
        /// <param name="key">Chiave di cache</param>
        Task<T?> GetAsync<T>(string key);

        /// <summary>
        /// Memorizza un valore nella cache (asincrono)
        /// </summary>
        /// <typeparam name="T">Tipo del valore</typeparam>
        /// <param name="key">Chiave di cache</param>
        /// <param name="value">Valore da memorizzare</param>
        /// <param name="expiration">Durata di validità</param>
        Task SetAsync<T>(string key, T value, TimeSpan expiration);

        /// <summary>
        /// Rimuove una chiave dalla cache (asincrono)
        /// </summary>
        /// <param name="key">Chiave da rimuovere</param>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// Verifica l'esistenza di una chiave (asincrono)
        /// </summary>
        /// <param name="key">Chiave da verificare</param>
        Task<bool> ExistsAsync(string key);
    }
}