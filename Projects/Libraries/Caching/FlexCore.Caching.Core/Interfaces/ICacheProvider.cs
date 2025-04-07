using System.Threading.Tasks;

namespace FlexCore.Caching.Core.Interfaces
{
    /// <summary>
    /// Definisce il contratto base per tutti i provider di cache
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Verifica l'esistenza di una chiave nella cache
        /// </summary>
        /// <param name="key">Chiave da verificare</param>
        /// <returns>Task che restituisce true se la chiave esiste</returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// Ottiene un valore dalla cache
        /// </summary>
        /// <typeparam name="T">Tipo del valore da recuperare</typeparam>
        /// <param name="key">Chiave associata al valore</param>
        /// <returns>Task che restituisce il valore o default(T)</returns>
        Task<T?> GetAsync<T>(string key);

        /// <summary>
        /// Memorizza un valore con scadenza
        /// </summary>
        /// <typeparam name="T">Tipo del valore da memorizzare</typeparam>
        /// <param name="key">Chiave associata al valore</param>
        /// <param name="value">Valore da memorizzare</param>
        /// <param name="expiry">Durata prima della scadenza</param>
        /// <returns>Task che restituisce true se l'operazione è riuscita</returns>
        Task<bool> SetAsync<T>(string key, T value, TimeSpan expiry);

        /// <summary>
        /// Rimuove una chiave dalla cache
        /// </summary>
        /// <param name="key">Chiave da rimuovere</param>
        /// <returns>Task che restituisce true se la chiave è stata rimossa</returns>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// Svuota completamente la cache
        /// </summary>
        /// <returns>Task che rappresenta il completamento dell'operazione</returns>
        Task ClearAllAsync();
    }
}