namespace FlexCore.Logging.Interfaces;

/// <summary>
/// Interfaccia per il sistema di logging.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Registra un messaggio di debug.
    /// </summary>
    /// <param name="message">Messaggio da registrare.</param>
    void Debug(string message);

    /// <summary>
    /// Registra un messaggio informativo.
    /// </summary>
    /// <param name="message">Messaggio da registrare.</param>
    void Info(string message);

    /// <summary>
    /// Registra un messaggio di avvertimento.
    /// </summary>
    /// <param name="message">Messaggio da registrare.</param>
    void Warn(string message);

    /// <summary>
    /// Registra un messaggio di errore.
    /// </summary>
    /// <param name="message">Messaggio da registrare.</param>
    void Error(string message);

    /// <summary>
    /// Registra un messaggio di errore fatale.
    /// </summary>
    /// <param name="message">Messaggio da registrare.</param>
    void Fatal(string message);
}