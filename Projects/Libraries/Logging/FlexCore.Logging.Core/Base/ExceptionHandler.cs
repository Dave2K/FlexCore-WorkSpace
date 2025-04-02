using Microsoft.Extensions.Logging;
using System;

namespace FlexCore.Logging.Core.Base
{
    /// <summary>
    /// Gestore avanzato per la gestione delle eccezioni nel logging.
    /// </summary>
    public static class ExceptionHandler
    {
        /// <summary>
        /// Gestisce un'eccezione e restituisce un'istanza tipizzata con logging.
        /// </summary>
        /// <typeparam name="TException">Tipo di eccezione da sollevare.</typeparam>
        /// <param name="logger">Istanza del logger.</param>
        /// <param name="ex">Eccezione originale.</param>
        /// <param name="operation">Operazione durante la quale si è verificato l'errore.</param>
        /// <param name="context">Contesto aggiuntivo (opzionale).</param>
        public static TException HandleException<TException>(
            ILogger logger,
            Exception ex,
            string operation,
            string context = "")
            where TException : Exception
        {
            string message = $"Errore durante {operation}";
            if (!string.IsNullOrEmpty(context))
                message += $" (Contesto: {context})";

            logger.LogError(ex, message);

            return (TException)Activator.CreateInstance(
                typeof(TException),
                message,
                ex)!;
        }
    }
}