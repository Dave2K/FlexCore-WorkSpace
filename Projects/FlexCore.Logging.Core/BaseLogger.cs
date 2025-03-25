namespace FlexCore.Logging.Core;

using FlexCore.Logging.Interfaces;

/// <summary>
/// Classe base astratta per i logger.
/// </summary>
public abstract class BaseLogger : ILogger
{
    /// <summary>
    /// Registra un messaggio di debug.
    /// </summary>
    /// <param name="message">Messaggio da registrare.</param>
    public void Debug(string message) => Log("DEBUG", message);

    /// <summary>
    /// Registra un messaggio informativo.
    /// </summary>
    /// <param name="message">Messaggio da registrare.</param>
    public void Info(string message) => Log("INFO", message);

    /// <summary>
    /// Registra un messaggio di avvertimento.
    /// </summary>
    /// <param name="message">Messaggio da registrare.</param>
    public void Warn(string message) => Log("WARN", message);

    /// <summary>
    /// Registra un messaggio di errore.
    /// </summary>
    /// <param name="message">Messaggio da registrare.</param>
    public void Error(string message) => Log("ERROR", message);

    /// <summary>
    /// Registra un messaggio di errore fatale.
    /// </summary>
    /// <param name="message">Messaggio da registrare.</param>
    public void Fatal(string message) => Log("FATAL", message);

    /// <summary>
    /// Metodo astratto per la registrazione dei messaggi.
    /// </summary>
    /// <param name="level">Livello di log.</param>
    /// <param name="message">Messaggio da registrare.</param>
    protected abstract void Log(string level, string message);
}