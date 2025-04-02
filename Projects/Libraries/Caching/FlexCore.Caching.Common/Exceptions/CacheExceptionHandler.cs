using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;

namespace FlexCore.Caching.Common.Exceptions
{
    /// <summary>
    /// Gestore centralizzato per eccezioni di caching con logging contestualizzato.
    /// </summary>
    public static class CacheExceptionHandler
    {
        /// <summary>
        /// Gestisce un'eccezione di caching e restituisce un'istanza tipizzata.
        /// </summary>
        /// <typeparam name="TException">Tipo di eccezione da sollevare.</typeparam>
        /// <param name="logger">Istanza del logger.</param>
        /// <param name="ex">Eccezione originale.</param>
        /// <param name="operation">Operazione fallita (es. "lettura cache").</param>
        /// <param name="key">Chiave di cache coinvolta (opzionale).</param>
        /// <param name="caller">Metodo chiamante (auto-compilato).</param>
        public static TException HandleException<TException>(
            ILogger logger,
            Exception ex,
            string operation,
            string? key = null,
            [CallerMemberName] string caller = "")
            where TException : CacheException
        {
            string message = $"[{caller}] Errore durante {operation}";
            if (!string.IsNullOrEmpty(key))
                message += $" (Key: {key})";

            logger.LogError(ex, message);

            var exception = Activator.CreateInstance(
                typeof(TException),
                message,
                ex) as TException;

            return exception ?? throw new InvalidOperationException("Creazione eccezione fallita");
        }
    }
}