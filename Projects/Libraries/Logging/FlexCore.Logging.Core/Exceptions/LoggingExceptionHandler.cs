using Microsoft.Extensions.Logging;
using System;

namespace FlexCore.Logging.Core.Exceptions
{
    /// <summary>
    /// Gestore specializzato per eccezioni di logging
    /// </summary>
    public static class LoggingExceptionHandler
    {
        /// <summary>
        /// Gestisce eccezioni di logging con contesto specifico
        /// </summary>
        public static TException HandleLoggingException<TException>(
            ILogger logger,
            Exception ex,
            string component,
            string operation)
            where TException : Exception
        {
            string message = $"[{component}] Errore durante {operation}";
            logger.LogError(ex, message);

            return (TException)Activator.CreateInstance(
                typeof(TException),
                message,
                ex
            )!;
        }
    }
}