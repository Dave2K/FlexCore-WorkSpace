namespace FlexCore.Logging.Log4Net;

using FlexCore.Logging.Core;
using FlexCore.Logging.Interfaces;
using log4net;

/// <summary>
/// Provider di logging con Log4Net.
/// </summary>
public class Log4NetLogger : BaseLogger, ILoggingProvider
{
    private readonly ILog _logger;

    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="Log4NetLogger"/>.
    /// </summary>
    public Log4NetLogger()
    {
        _logger = LogManager.GetLogger(typeof(Log4NetLogger));
    }

    /// <summary>
    /// Registra un messaggio utilizzando Log4Net.
    /// </summary>
    /// <param name="level">Livello di log.</param>
    /// <param name="message">Messaggio da registrare.</param>
    protected override void Log(string level, string message)
    {
        switch (level)
        {
            case "DEBUG":
                _logger.Debug(message);
                break;
            case "INFO":
                _logger.Info(message);
                break;
            case "WARN":
                _logger.Warn(message);
                break;
            case "ERROR":
                _logger.Error(message);
                break;
            case "FATAL":
                _logger.Fatal(message);
                break;
        }
    }
}