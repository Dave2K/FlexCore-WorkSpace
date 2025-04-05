using FlexCore.Caching.Common.Exceptions;
using Microsoft.Extensions.Logging;
using System;

namespace FlexCore.Caching.Redis
{
    /// <summary>
    /// Eccezione sollevata per errori specifici del provider Redis.
    /// </summary>
    /// <remarks>
    /// Estende <see cref="CacheException"/> per fornire contesto aggiuntivo.
    /// </remarks>
    public class RedisCacheException : CacheException
    {
        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="RedisCacheException"/>.
        /// </summary>
        /// <param name="logger">Logger per tracciare l'errore.</param>
        /// <param name="message">Messaggio descrittivo dell'errore.</param>
        /// <param name="inner">Eccezione interna originale.</param>
        public RedisCacheException(
            ILogger<RedisCacheException> logger,
            string message,
            Exception inner)
            : base(message, inner)
        {
            logger.LogError(inner, "Errore Redis: {Message}", message);
        }

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="RedisCacheException"/>.
        /// </summary>
        /// <param name="message">Messaggio descrittivo dell'errore.</param>
        /// <param name="inner">Eccezione interna originale.</param>
        public RedisCacheException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}