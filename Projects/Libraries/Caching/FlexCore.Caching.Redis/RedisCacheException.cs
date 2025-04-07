using Microsoft.Extensions.Logging;
using System;

namespace FlexCore.Caching.Redis
{
    /// <summary>
    /// Eccezione specializzata per errori Redis
    /// </summary>
    public class RedisCacheException : Exception
    {
        /// <summary>
        /// Inizializza una nuova istanza
        /// </summary>
        public RedisCacheException(string message, Exception inner)
            : base(ValidateMessage(message), inner) { }

        /// <summary>
        /// Inizializza una nuova istanza con logging integrato
        /// </summary>
        public RedisCacheException(
            ILogger<RedisCacheProvider> logger,
            string message,
            Exception inner) : base(ValidateMessage(message), inner)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            logger.LogError(
                "Errore Redis - Messaggio: {Message}, Tipo: {Type}, Stack: {Stack}",
                message,
                inner.GetType().FullName,
                inner.StackTrace ?? "N/A");
        }

        /// <summary>
        /// Inizializza una nuova istanza senza inner exception
        /// </summary>
        public RedisCacheException(string message)
            : base(ValidateMessage(message)) { }

        private static string ValidateMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException(
                    "Il messaggio deve contenere informazioni significative",
                    nameof(message));

            return message;
        }
    }
}