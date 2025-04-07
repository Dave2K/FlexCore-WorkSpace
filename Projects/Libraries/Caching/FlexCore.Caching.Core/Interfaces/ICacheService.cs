using System.Threading.Tasks;

namespace FlexCore.Caching.Core.Interfaces
{
    /// <summary>
    /// Definisce il contratto per un servizio di cache con operazioni asincrone.
    /// Tutte le implementazioni devono garantire la gestione thread-safe e il rispetto delle politiche di scadenza.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Ottiene un valore dalla cache in modo asincrono.
        /// </summary>
        /// <typeparam name="T">Tipo del valore memorizzato.</typeparam>
        /// <param name="key">Chiave identificativa del valore.</param>
        /// <returns>
        /// Il valore associato alla chiave, o <c>default(T)</c> se la chiave non esiste o è scaduta.
        /// </returns>
        Task<T?> GetAsync<T>(string key);

        /// <summary>
        /// Memorizza un valore nella cache con scadenza in modo asincrono.
        /// </summary>
        /// <typeparam name="T">Tipo del valore da memorizzare.</typeparam>
        /// <param name="key">Chiave identificativa.</param>
        /// <param name="value">Valore da memorizzare.</param>
        /// <param name="expiration">Intervallo di tempo dopo il quale il valore scade.</param>
        /// <returns><c>true</c> se l'operazione è riuscita, <c>false</c> altrimenti.</returns>
        Task<bool> SetAsync<T>(string key, T value, TimeSpan expiration);

        /// <summary>
        /// Rimuove una chiave dalla cache in modo asincrono.
        /// </summary>
        /// <param name="key">Chiave da rimuovere.</param>
        /// <returns><c>true</c> se la chiave è stata rimossa, <c>false</c> se non esisteva.</returns>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// Verifica l'esistenza di una chiave nella cache in modo asincrono.
        /// </summary>
        /// <param name="key">Chiave da verificare.</param>
        /// <returns><c>true</c> se la chiave esiste, <c>false</c> altrimenti.</returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// Svuota completamente la cache in modo asincrono.
        /// </summary>
        /// <returns>Operazione completata.</returns>
        Task ClearAllAsync();
    }
}