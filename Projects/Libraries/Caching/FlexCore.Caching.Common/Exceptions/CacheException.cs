using System;

namespace FlexCore.Caching.Common.Exceptions
{
    /// <summary>
    /// Eccezione base per tutti gli errori relativi alle operazioni di caching.
    /// </summary>
    public class CacheException : Exception
    {
        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="CacheException"/>.
        /// </summary>
        /// <param name="message">Messaggio che descrive l'errore.</param>
        /// <param name="innerException">Eccezione interna.</param>
        /// <exception cref="ArgumentException">Se <paramref name="message"/> è vuoto.</exception>
        public CacheException(string message, Exception innerException)
            : base(message, innerException)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Il messaggio deve contenere informazioni significative", nameof(message)); // ✅
        }

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="CacheException"/>.
        /// </summary>
        /// <param name="message">Messaggio che descrive l'errore.</param>
        /// <exception cref="ArgumentException">Se <paramref name="message"/> è vuoto.</exception>
        public CacheException(string message)
            : base(message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Il messaggio deve contenere informazioni significative", nameof(message)); // ✅
        }
    }
}