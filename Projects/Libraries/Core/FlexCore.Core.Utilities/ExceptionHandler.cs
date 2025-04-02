using Microsoft.Extensions.Logging;

namespace FlexCore.Core.Utilities
{
    /// <summary>
    /// Gestore centralizzato delle eccezioni
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
        /// Wrap un'eccezione in un tipo specifico
        /// </summary>
        public static TE WrapException<TE, T>(
            this ILogger<T> logger,
            Exception ex,
            string message) where TE : Exception
        {
            logger.LogException(ex, message);
            return (TE)Activator.CreateInstance(typeof(TE), message, ex)!;
        }
    }
}