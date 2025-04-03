using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;

namespace FlexCore.Core.Utilities
{
    /// <summary>
    /// Fornisce metodi statici per la gestione centralizzata delle eccezioni
    /// </summary>
    public static class ExceptionHandler
    {
        /// <summary>
        /// Gestisce un'eccezione e restituisce un'istanza tipizzata dell'eccezione specificata
        /// </summary>
        /// <typeparam name="TException">Tipo di eccezione da creare</typeparam>
        /// <param name="logger">Istanza del logger per il tracciamento</param>
        /// <param name="ex">Eccezione originale</param>
        /// <param name="operation">Descrizione dell'operazione fallita</param>
        /// <param name="context">Contesto aggiuntivo opzionale</param>
        /// <param name="caller">Nome del metodo chiamante (compilato automaticamente)</param>
        /// <returns>Istanza dell'eccezione creata</returns>
        /// <exception cref="InvalidOperationException">Se il costruttore richiesto non esiste</exception>
        public static TException HandleException<TException>(
            ILogger logger,
            Exception ex,
            string operation,
            string? context = null,
            [CallerMemberName] string caller = "")
            where TException : Exception
        {
            string message = $"Errore durante {operation}";
            if (!string.IsNullOrEmpty(context))
                message += $" (Contesto: {context})";

            logger.LogError(ex, $"[{caller}] {message}");

            try
            {
                return (TException)Activator.CreateInstance(
                    typeof(TException),
                    message,
                    ex)!;
            }
            catch (MissingMethodException)
            {
                throw new InvalidOperationException(
                    $"Il tipo {typeof(TException)} deve avere un costruttore con parametri (string, Exception)", ex);
            }
        }
    }
}