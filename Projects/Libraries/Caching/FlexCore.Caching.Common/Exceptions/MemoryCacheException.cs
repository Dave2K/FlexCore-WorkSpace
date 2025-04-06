using Microsoft.Extensions.Logging;
using System;

namespace FlexCore.Caching.Common.Exceptions
{
    /// <summary>
    /// Eccezione sollevata per errori specifici nelle operazioni di MemoryCache
    /// </summary>
    /// <remarks>
    /// <para>
    /// Fornisce contesto aggiuntivo per errori nella gestione della cache in memoria,
    /// inclusi dettagli diagnostici come:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Nome del metodo chiamante</description></item>
    /// <item><description>Chiave della cache coinvolta</description></item>
    /// <item><description>Tipo dell'eccezione originale</description></item>
    /// </list>
    /// </remarks>
    public class MemoryCacheException : CacheException
    {
        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="MemoryCacheException"/>
        /// </summary>
        /// <param name="message">Messaggio descrittivo dell'errore</param>
        /// <param name="inner">Eccezione interna originale</param>
        /// <exception cref="ArgumentException">
        /// Se <paramref name="message"/> è vuoto o contiene solo spazi bianchi
        /// </exception>
        public MemoryCacheException(string message, Exception inner)
            : base(message, inner)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Il messaggio deve contenere informazioni significative", nameof(message));
        }

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="MemoryCacheException"/> con logging integrato
        /// </summary>
        /// <param name="logger">Istanza del logger per il tracciamento</param>
        /// <param name="message">Messaggio descrittivo dell'errore</param>
        /// <param name="inner">Eccezione interna originale</param>
        /// <exception cref="ArgumentNullException">Se <paramref name="logger"/> è null</exception>
        public MemoryCacheException(
            ILogger<MemoryCacheException> logger,
            string message,
            Exception inner)
            : this(message, inner)
        {
            if (logger is null)
                throw new ArgumentNullException(nameof(logger), "Il logger è obbligatorio");

            logger.LogError(
                "Errore MemoryCache - Messaggio: {ErrorMessage}, Tipo: {ExceptionType}, Stack: {Stack}",
                message,
                inner.GetType().FullName,
                inner.StackTrace);
        }

        /// <summary>
        /// Ottiene la rappresentazione formattata dello stack trace combinato
        /// </summary>
        /// <value>
        /// Stringa formattata che combina lo stack trace corrente e quello dell'eccezione interna
        /// </value>
        public string FullStackTrace =>
            $"Stack corrente: {StackTrace}\nStack interno: {InnerException?.StackTrace}";
    }
}