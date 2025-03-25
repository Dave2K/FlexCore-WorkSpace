namespace FlexCore.Core.Utilities;

using System;

/// <summary>
/// Classe statica per la gestione generica delle eccezioni.
/// </summary>
public static class ExceptionHandler
{
    /// <summary>
    /// Gestisce le eccezioni durante le operazioni.
    /// </summary>
    /// <param name="ex">L'eccezione catturata.</param>
    /// <param name="operation">L'operazione che ha generato l'eccezione.</param>
    /// <param name="customExceptionFactory">Funzione per creare eccezioni personalizzate.</param>
    /// <exception cref="Exception">Se si verifica un errore durante l'operazione.</exception>
    public static void HandleException(Exception ex, string operation, Func<Exception, string, Exception> customExceptionFactory)
    {
        throw customExceptionFactory(ex, operation);
    }
}