namespace FleFlexCore.Logging.Core.Base;

using FlexCore.Logging.Interfaces;

/// <summary>
/// Classe base astratta per i provider di logging.
/// </summary>
public abstract class LoggingProviderBase : ILoggingProvider
{
    /// <summary>
    /// Registra un messaggio di debug.
    /// </summary>
    /// <param name="message">Messaggio da registrare.</param>
    public abstract void Debug(string message);

    /// <summary>
    /// Registra un messaggio informativo.
    /// </summary>
    /// <param name="message">Messaggio da registrare.</param>
    public abstract void Info(string message);

    /// <summary>
    /// Registra un messaggio di avvertimento.
    /// </summary>
    /// <param name="message">Messaggio da registrare.</param>
    public abstract void Warn(string message);

    /// <summary>
    /// Registra un messaggio di errore.
    /// </summary>
    /// <param name="message">Messaggio da registrare.</param>
    public abstract void Error(string message);

    /// <summary>
    /// Registra un messaggio di errore fatale.
    /// </summary>
    /// <param name="message">Messaggio da registrare.</param>
    public abstract void Fatal(string message);
}