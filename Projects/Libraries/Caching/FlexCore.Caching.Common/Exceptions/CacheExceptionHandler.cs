using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;

namespace FlexCore.Caching.Common.Exceptions
{
    /// <summary>
    /// Fornisce metodi statici per la gestione centralizzata delle eccezioni di caching.
    /// </summary>
    public static class CacheExceptionHandler
    {
        /// <summary>
        /// Gestisce un'eccezione e restituisce un'istanza tipizzata dell'eccezione specificata.
        /// </summary>
        /// <typeparam name="TException">Tipo di eccezione derivante da <see cref="CacheException"/>.</typeparam>
        /// <param name="logger">Istanza del logger per il tracciamento.</param>
        /// <param name="ex">Eccezione originale.</param>
        /// <param name="operation">Operazione fallita (es. "lettura cache").</param>
        /// <param name="key">Chiave di cache coinvolta (opzionale).</param>
        /// <param name="caller">Nome del metodo chiamante (compilato automaticamente).</param>
        /// <returns>Istanza dell'eccezione creata.</returns>
        /// <exception cref="ArgumentNullException">Se <paramref name="logger"/> o <paramref name="ex"/> sono null.</exception>
        /// <exception cref="ArgumentException">Se <paramref name="operation"/> è vuoto o spazi bianchi.</exception>
        /// <exception cref="InvalidOperationException">Se il costruttore richiesto non esiste.</exception>
        public static TException HandleException<TException>(
            ILogger logger,
            Exception ex,
            string operation,
            string? key = null,
            [CallerMemberName] string caller = "")
            where TException : CacheException
        {
            // Validazione input
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (ex == null)
                throw new ArgumentNullException(nameof(ex));
            if (string.IsNullOrWhiteSpace(operation))
                throw new ArgumentException("Operation non può essere vuoto", nameof(operation));

            // Costruzione messaggio di log
            string message = $"[{caller}] Errore durante {operation}";
            if (!string.IsNullOrWhiteSpace(key))
                message += $" (Key: {key})";

            // Logging dell'errore
            logger.LogError(ex, message);

            try
            {
                // Creazione eccezione tipizzata tramite reflection
                return (TException)Activator.CreateInstance(
                    typeof(TException),
                    message,
                    ex)!;
            }
            catch (MissingMethodException)
            {
                throw new InvalidOperationException(
                    $"{typeof(TException)} deve avere un costruttore con parametri (string, Exception)", ex);
            }
        }
    }
}