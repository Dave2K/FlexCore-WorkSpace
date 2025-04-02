using Microsoft.Extensions.Logging;

namespace FlexCore.Caching.Common.Exceptions
{
    /// <summary>
    /// Eccezione specifica per errori nella cache Redis.
    /// </summary>
    public class RedisCacheException : CacheException
    {
        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="RedisCacheException"/>.
        /// </summary>
        /// <param name="logger">Logger per tracciare l'errore.</param>
        /// <param name="message">Messaggio descrittivo dell'errore.</param>
        /// <param name="inner">Eccezione interna.</param>
        public RedisCacheException(
            ILogger<RedisCacheException> logger,
            string message,
            Exception inner)
            : base(message, inner)
        {
            logger.LogError(inner, message);
        }
    }
}