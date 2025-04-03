namespace FlexCore.Logging.Serilog;

using FleFlexCore.Logging.Core.Base;
using FlexCore.Logging.Interfaces;

/// <summary>
/// Provider di logging con Serilog.
/// </summary>
public class SerilogLogger : BaseLogger, ILoggingProvider
{
    private readonly global::Serilog.ILogger _logger;

    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="SerilogLogger"/>.
    /// </summary>
    public SerilogLogger()
    {
        _logger = global::Serilog.Log.ForContext<SerilogLogger>();
    }

    /// <summary>
    /// Registra un messaggio utilizzando Serilog.
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
                _logger.Information(message);
                break;
            case "WARN":
                _logger.Warning(message);
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