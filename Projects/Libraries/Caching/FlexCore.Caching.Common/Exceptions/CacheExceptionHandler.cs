using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;

namespace FlexCore.Caching.Common.Exceptions
{
    /// <summary>
    /// Fornisce metodi per la gestione centralizzata delle eccezioni relative alle operazioni di caching
    /// </summary>
    /// <remarks>
    /// Implementa un pattern factory per la creazione di eccezioni tipizzate con:
    /// <list type="bullet">
    /// <item><description>Logging integrato</description></item>
    /// <item><description>Tracciamento del contesto</description></item>
    /// <item><description>Validazione degli input</description></item>
    /// <item><description>Mantenimento dello stack trace originale</description></item>
    /// </list>
    /// </remarks>
    public static class CacheExceptionHandler
    {
        /// <summary>
        /// Crea e registra un'eccezione specifica per errori di caching
        /// </summary>
        /// <typeparam name="TException">Tipo di eccezione derivante da <see cref="CacheException"/></typeparam>
        /// <param name="logger">Istanza del logger per il tracciamento</param>
        /// <param name="ex">Eccezione originale</param>
        /// <param name="operation">Tipo di operazione fallita (es. "Lettura cache")</param>
        /// <param name="key">Chiave di cache coinvolta (opzionale)</param>
        /// <param name="caller">Nome del metodo chiamante (automatico tramite CallerMemberName)</param>
        /// <returns>Istanza dell'eccezione creata</returns>
        /// <exception cref="ArgumentNullException">Se logger o ex sono null</exception>
        /// <exception cref="ArgumentException">Se operation è vuoto</exception>
        /// <exception cref="InvalidOperationException">Se il costruttore richiesto è mancante</exception>
        public static TException HandleException<TException>(
            ILogger logger,
            Exception ex,
            string operation,
            string? key = null,
            [CallerMemberName] string caller = "")
            where TException : CacheException
        {
            if (logger is null)
                throw new ArgumentNullException(nameof(logger), "Il logger è obbligatorio");

            if (ex is null)
                throw new ArgumentNullException(nameof(ex), "L'eccezione originale è obbligatoria");

            if (string.IsNullOrWhiteSpace(operation))
                throw new ArgumentException("Specificare un'operazione valida", nameof(operation));

            var message = $"[{caller}] Operazione fallita: {operation}";
            if (!string.IsNullOrWhiteSpace(key))
                message += $" | Chiave: {key}";

            logger.LogError(ex, "{ErrorMessage}", message);

            try
            {
                return (TException)Activator.CreateInstance(
                    typeof(TException),
                    message,
                    ex
                )!;
            }
            catch (MissingMethodException mme)
            {
                throw new InvalidOperationException(
                    $"Costruttore (string, Exception) mancante in {typeof(TException).Name}",
                    mme);
            }
        }
    }
}