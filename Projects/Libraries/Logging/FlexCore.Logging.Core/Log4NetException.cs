namespace FlexCore.Logging.Core;

using System;

/// <summary>
/// Eccezione specifica per errori di Log4Net.
/// </summary>
public class Log4NetException : Exception
{
    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="Log4NetException"/>.
    /// </summary>
    /// <param name="message">Messaggio di errore.</param>
    public Log4NetException(string message) : base(message) { }

    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="Log4NetException"/>.
    /// </summary>
    /// <param name="message">Messaggio di errore.</param>
    /// <param name="innerException">Eccezione interna.</param>
    public Log4NetException(string message, Exception innerException) : base(message, innerException) { }
}