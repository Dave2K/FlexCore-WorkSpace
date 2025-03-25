namespace FlexCore.Core.Configuration.Models;

using System.Collections.Generic;

/// <summary>
/// Impostazioni di logging.
/// </summary>
public class LoggingSettings
{
    /// <summary>
    /// Abilita il logging.
    /// </summary>
    public required bool Enabled { get; set; }

    /// <summary>
    /// Livello di logging (Trace, Debug, Information, Warning, Error, Critical).
    /// </summary>
    public required string Level { get; set; }

    /// <summary>
    /// Lista dei provider di logging supportati.
    /// </summary>
    public List<string> Providers { get; set; } = new List<string>();

    /// <summary>
    /// Impostazioni specifiche per il logging su console.
    /// </summary>
    public required ConsoleLoggingSettings Console { get; set; }

    /// <summary>
    /// Impostazioni specifiche per il logging con Log4Net.
    /// </summary>
    public required Log4NetSettings Log4Net { get; set; }

    /// <summary>
    /// Impostazioni specifiche per il logging con Serilog.
    /// </summary>
    public required SerilogSettings Serilog { get; set; }
}

/// <summary>
/// Impostazioni specifiche per il logging su console.
/// </summary>
public class ConsoleLoggingSettings
{
    /// <summary>
    /// Includi informazioni sugli scope nei log.
    /// </summary>
    public required bool IncludeScopes { get; set; }

    /// <summary>
    /// Impostazioni del livello di logging.
    /// </summary>
    public required LogLevelSettings LogLevel { get; set; }
}

/// <summary>
/// Impostazioni del livello di logging.
/// </summary>
public class LogLevelSettings
{
    /// <summary>
    /// Livello di logging predefinito.
    /// </summary>
    public required string Default { get; set; }

    /// <summary>
    /// Livello di logging per i messaggi di sistema.
    /// </summary>
    public required string System { get; set; }

    /// <summary>
    /// Livello di logging per i messaggi di Microsoft.
    /// </summary>
    public required string Microsoft { get; set; }
}

/// <summary>
/// Impostazioni specifiche per il logging con Log4Net.
/// </summary>
public class Log4NetSettings
{
    /// <summary>
    /// File di configurazione Log4Net.
    /// </summary>
    public required string ConfigFile { get; set; }

    /// <summary>
    /// Impostazioni del livello di logging.
    /// </summary>
    public required LogLevelSettings LogLevel { get; set; }
}

/// <summary>
/// Impostazioni specifiche per il logging con Serilog.
/// </summary>
public class SerilogSettings
{
    /// <summary>
    /// Sink di Serilog (Console e File).
    /// </summary>
    public List<string> Using { get; set; } = new List<string>();

    /// <summary>
    /// Impostazioni del livello minimo di logging.
    /// </summary>
    public required MinimumLevelSettings MinimumLevel { get; set; }

    /// <summary>
    /// Impostazioni per la scrittura dei log.
    /// </summary>
    public List<WriteToSettings> WriteTo { get; set; } = new List<WriteToSettings>();
}

/// <summary>
/// Impostazioni del livello minimo di logging.
/// </summary>
public class MinimumLevelSettings
{
    /// <summary>
    /// Livello di logging predefinito.
    /// </summary>
    public required string Default { get; set; }

    /// <summary>
    /// Override del livello di logging per specifici namespace.
    /// </summary>
    public Dictionary<string, string> Override { get; set; } = new Dictionary<string, string>();
}

/// <summary>
/// Impostazioni per la scrittura dei log.
/// </summary>
public class WriteToSettings
{
    /// <summary>
    /// Nome del sink (es. "Console", "File").
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Argomenti per il sink.
    /// </summary>
    public Dictionary<string, object> Args { get; set; } = new Dictionary<string, object>();
}