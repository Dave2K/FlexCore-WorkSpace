using System;
using FlexCore.Core.Utilities;

namespace FlexCore.Logging.Core;

/// <summary>
/// Classe statica per la gestione delle eccezioni relative al logging
/// </summary>
public static class LoggingExceptionHandler
{
    /// <summary>
    /// Gestisce le eccezioni durante le operazioni di logging
    /// </summary>
    /// <param name="ex">Eccezione da gestire</param>
    /// <param name="operation">Nome dell'operazione fallita</param>
    /// <exception cref="LoggingException">Sempre lanciata per incapsulare l'eccezione originale</exception>
    public static void HandleException(Exception ex, string operation)
    {
        ExceptionHandler.HandleException(
            ex,
            operation,
            (e, op) => e switch
            {
                Log4NetException => new LoggingException($"Errore di Log4Net durante {op}", e),
                SerilogException => new LoggingException($"Errore di Serilog durante {op}", e),
                _ => new LoggingException($"Errore durante l'operazione di logging: {op}", e)
            });
    }
}