using Microsoft.Extensions.Logging;
using System;

namespace FlexCore.Core.Utilities
{
    /// <summary>
    /// Gestore centralizzato delle eccezioni con supporto per logging e wrapping
    /// </summary>
    public static class ExceptionHandler
    {
        /// <summary>
        /// Logga un'eccezione in modo standardizzato
        /// </summary>
        public static void LogException<T>(
            this ILogger<T> logger,
            Exception ex,
            string customMessage = "")
        {
            logger.LogError(ex, $"ERRORE: {customMessage} | {ex.GetBaseException().Message}");
        }

        /// <summary>
        /// Wrap un'eccezione in un tipo specifico con logging contestuale
        /// </summary>
        public static TE WrapException<TE, T>(
            this ILogger<T> logger,
            Exception ex,
            string message) where TE : Exception
        {
            logger.LogException(ex, message);
            return (TE)Activator.CreateInstance(typeof(TE), message, ex)!;
        }

        /// <summary>
        /// Gestione avanzata di eccezioni con creazione tipizzata
        /// </summary>
        public static TException HandleException<TException>(
            ILogger logger,
            Exception ex,
            string operation,
            string context = "") where TException : Exception
        {
            string message = $"Errore durante {operation}";
            if (!string.IsNullOrEmpty(context))
                message += $" (Contesto: {context})";

            logger.LogError(ex, message);
            return (TException)Activator.CreateInstance(typeof(TException), message, ex)!;
        }
    }
}