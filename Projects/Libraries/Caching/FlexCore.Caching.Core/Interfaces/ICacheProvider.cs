namespace FlexCore.Caching.Core.Interfaces
{
    /// <summary>
    /// Interfaccia base per i provider di cache con metodi asincroni
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Verifica in modo asincrono se una chiave esiste nella cache
        /// </summary>
        /// <param name="key">Chiave da verificare</param>
        /// <returns>Task che restituisce true se la chiave esiste</returns>
        /// <exception cref="ArgumentException">Se la chiave è nulla o vuota</exception>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// Ottiene in modo asincrono un valore dalla cache
        /// </summary>
        /// <typeparam name="T">Tipo dell'oggetto da deserializzare</typeparam>
        /// <param name="key">Chiave associata al valore</param>
        /// <returns>Valore deserializzato o default</returns>
        /// <exception cref="ArgumentException">Se la chiave è nulla o vuota</exception>
        Task<T?> GetAsync<T>(string key);

        /// <summary>
        /// Imposta in modo asincrono un valore nella cache
        /// </summary>
        /// <typeparam name="T">Tipo dell'oggetto da serializzare</typeparam>
        /// <param name="key">Chiave associata al valore</param>
        /// <param name="value">Valore da memorizzare</param>
        /// <param name="expiry">Durata di validità</param>
        /// <returns>Task che indica l'esito dell'operazione</returns>
        /// <exception cref="ArgumentException">Se la chiave è nulla o vuota</exception>
        Task<bool> SetAsync<T>(string key, T value, TimeSpan expiry);

        /// <summary>
        /// Rimuove in modo asincrono un valore dalla cache
        /// </summary>
        /// <param name="key">Chiave da rimuovere</param>
        /// <returns>Task che indica l'esito dell'operazione</returns>
        /// <exception cref="ArgumentException">Se la chiave è nulla o vuota</exception>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// Svuota completamente la cache
        /// </summary>
        /// <returns>Task che rappresenta l'operazione</returns>
        Task ClearAllAsync();
    }
}