using System;

namespace FlexCore.Caching.Common.Exceptions
{
    /// <summary>
    /// Eccezione base per tutti gli errori relativi alle operazioni di caching
    /// </summary>
    /// <remarks>
    /// Questa eccezione dovrebbe essere utilizzata come classe base per tutte le eccezioni specifiche della cache.
    /// </remarks>
    public class CacheException : Exception
    {
        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="CacheException"/>
        /// </summary>
        /// <param name="message">Messaggio che descrive l'errore</param>
        /// <param name="innerException">Eccezione interna che è causa dell'errore corrente</param>
        /// <exception cref="ArgumentNullException">Se <paramref name="message"/> è null o vuoto</exception>
        public CacheException(string message, Exception innerException)
            : base(message, innerException)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message), "Il messaggio non può essere vuoto");
        }

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="CacheException"/>
        /// </summary>
        /// <param name="message">Messaggio che descrive l'errore</param>
        /// <exception cref="ArgumentNullException">Se <paramref name="message"/> è null o vuoto</exception>
        public CacheException(string message)
            : base(message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message), "Il messaggio non può essere vuoto");
        }
    }
}