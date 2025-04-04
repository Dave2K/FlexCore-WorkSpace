using Microsoft.Extensions.Logging;

namespace FlexCore.Caching.Common.Exceptions
{
    /// <summary>
    /// Eccezione specifica per errori nella cache Redis
    /// </summary>
    public class RedisCacheException : CacheException
    {
        /// <summary>
        /// Inizializza una nuova istanza della classe
        /// </summary>
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