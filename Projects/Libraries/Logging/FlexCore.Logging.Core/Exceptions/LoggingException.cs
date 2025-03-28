namespace FleFlexCore.Logging.Core.Exceptions;

/// <summary>
/// Eccezione personalizzata per gli errori di logging.
/// </summary>
public class LoggingException : Exception
{
    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="LoggingException"/>.
    /// </summary>
    /// <param name="message">Messaggio di errore.</param>
    public LoggingException(string message) : base(message) { }

    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="LoggingException"/>.
    /// </summary>
    /// <param name="message">Messaggio di errore.</param>
    /// <param name="innerException">Eccezione interna.</param>
    public LoggingException(string message, Exception innerException) : base(message, innerException) { }
}