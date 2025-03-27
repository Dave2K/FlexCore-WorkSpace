namespace FlexCore.Logging.Core;

using System;

/// <summary>
/// Eccezione specifica per errori di Serilog.
/// </summary>
public class SerilogException : Exception
{
    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="SerilogException"/>.
    /// </summary>
    /// <param name="message">Messaggio di errore.</param>
    public SerilogException(string message) : base(message) { }

    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="SerilogException"/>.
    /// </summary>
    /// <param name="message">Messaggio di errore.</param>
    /// <param name="innerException">Eccezione interna.</param>
    public SerilogException(string message, Exception innerException) : base(message, innerException) { }
}