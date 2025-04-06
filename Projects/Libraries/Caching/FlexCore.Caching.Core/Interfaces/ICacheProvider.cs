using System;
using System.Threading.Tasks;

namespace FlexCore.Caching.Core.Interfaces
{
    /// <summary>
    /// Definisce il contratto per un provider di cache con operazioni asincrone
    /// </summary>
    /// <remarks>
    /// Implementa le operazioni CRUD asincrone per la gestione di dati in cache,
    /// supportando:
    /// <list type="bullet">
    /// <item><description>Scadenza automatica degli elementi</description></item>
    /// <item><description>Gestione centralizzata degli errori</description></item>
    /// <item><description>Tipizzazione forte dei valori</description></item>
    /// <item><description>Operazioni atomiche</description></item>
    /// </list>
    /// </remarks>
    public interface ICacheProvider
    {
        /// <summary>
        /// Verifica l'esistenza di una chiave nella cache
        /// </summary>
        /// <param name="key">Chiave da verificare</param>
        /// <returns>
        /// Task che restituisce true se la chiave esiste, altrimenti false
        /// </returns>
        /// <exception cref="ArgumentNullException">Se <paramref name="key"/> è null o vuoto</exception>
        /// <exception cref="CacheOperationException">Errore durante l'operazione</exception>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// Ottiene un valore dalla cache convertendolo nel tipo specificato
        /// </summary>
        /// <typeparam name="T">Tipo di destinazione per la conversione</typeparam>
        /// <param name="key">Chiave associata al valore</param>
        /// <returns>
        /// Task che restituisce il valore convertito o default(T) se non trovato
        /// </returns>
        /// <exception cref="ArgumentNullException">Se <paramref name="key"/> è null o vuoto</exception>
        /// <exception cref="InvalidCastException">Conversione di tipo fallita</exception>
        /// <exception cref="CacheOperationException">Errore durante la lettura</exception>
        Task<T?> GetAsync<T>(string key);

        /// <summary>
        /// Memorizza un valore nella cache con scadenza relativa
        /// </summary>
        /// <typeparam name="T">Tipo del valore da memorizzare</typeparam>
        /// <param name="key">Chiave univoca per l'identificazione</param>
        /// <param name="value">Valore da memorizzare</param>
        /// <param name="expiry">Durata di validità del dato</param>
        /// <returns>
        /// Task che restituisce true se l'operazione è riuscita
        /// </returns>
        /// <exception cref="ArgumentNullException">Se <paramref name="key"/> è null o vuoto</exception>
        /// <exception cref="CacheOperationException">Errore durante lo storage</exception>
        Task<bool> SetAsync<T>(string key, T value, TimeSpan expiry);

        /// <summary>
        /// Rimuove una chiave dalla cache
        /// </summary>
        /// <param name="key">Chiave da rimuovere</param>
        /// <returns>
        /// Task che restituisce true se la rimozione è riuscita
        /// </returns>
        /// <exception cref="ArgumentNullException">Se <paramref name="key"/> è null o vuoto</exception>
        /// <exception cref="CacheOperationException">Errore durante la cancellazione</exception>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// Svuota completamente la cache
        /// </summary>
        /// <remarks>
        /// Operazione potenzialmente costosa: usare con cautela in ambienti production
        /// </remarks>
        /// <exception cref="CacheOperationException">Errore durante lo svuotamento</exception>
        Task ClearAllAsync();
    }
}