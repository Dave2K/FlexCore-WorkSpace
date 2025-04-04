using System;
using System.Threading.Tasks;

namespace FlexCore.Caching.Core.Interfaces
{
    /// <summary>
    /// Definisce le operazioni base per un provider di cache
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Verifica l'esistenza di una chiave nella cache
        /// </summary>
        /// <param name="key">Chiave da verificare</param>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// Ottiene un valore dalla cache
        /// </summary>
        /// <typeparam name="T">Tipo del valore da recuperare</typeparam>
        /// <param name="key">Chiave associata al valore</param>
        Task<T?> GetAsync<T>(string key);

        /// <summary>
        /// Imposta un valore nella cache con scadenza
        /// </summary>
        /// <typeparam name="T">Tipo del valore da memorizzare</typeparam>
        /// <param name="key">Chiave associata al valore</param>
        /// <param name="value">Valore da memorizzare</param>
        /// <param name="expiry">Durata di validità</param>
        Task<bool> SetAsync<T>(string key, T value, TimeSpan expiry);

        /// <summary>
        /// Rimuove un valore dalla cache
        /// </summary>
        /// <param name="key">Chiave da rimuovere</param>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// Svuota completamente la cache
        /// </summary>
        Task ClearAllAsync();
    }
}