using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;

namespace FlexCore.Caching.Common.Exceptions
{
    /// <summary>  
    /// Fornisce metodi per la gestione centralizzata delle eccezioni di caching.  
    /// </summary>  
    public static class CacheExceptionHandler
    {
        /// <summary>  
        /// Crea e registra un'eccezione tipizzata per errori di caching.  
        /// </summary>  
        public static TException HandleException<TException>(
            ILogger logger,
            Exception ex,
            string operation,
            string? key = null,
            [CallerMemberName] string caller = "")
            where TException : CacheException
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (ex == null)
                throw new ArgumentNullException(nameof(ex));

            if (string.IsNullOrWhiteSpace(operation))
                throw new ArgumentException("Il nome dell'operazione non può essere vuoto.", nameof(operation));

            var message = $"[{caller}] Operazione fallita: {operation}{(key != null ? $" | Chiave: {key}" : "")}";
            logger.LogError(ex, "{ErrorMessage}", message);

            try
            {
                return (TException)Activator.CreateInstance(typeof(TException), message, ex)!;
            }
            catch (MissingMethodException)
            {
                throw new InvalidOperationException(
                    $"Il tipo {typeof(TException).Name} deve avere un costruttore (string, Exception).");
            }
        }
    }
}