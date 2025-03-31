namespace FlexCore.Core.Configuration.Models;

using System.Collections.Generic;

/// <summary>
/// Impostazioni di logging (allineate alla struttura JSON completa).
/// </summary>
public class LoggingSettings
{
    public required string DefaultProvider { get; set; }
    public List<string> Providers { get; set; } = new List<string>();
    public required bool Enabled { get; set; }
    public required string Level { get; set; }
    public required ConsoleLoggingSettings Console { get; set; }
    public required Log4NetSettings Log4Net { get; set; }
    public required SerilogSettings Serilog { get; set; }
}

/// <summary>
/// Impostazioni specifiche per il logging su console.
/// </summary>
public class ConsoleLoggingSettings
{
    public required bool IncludeScopes { get; set; }
    public required LogLevelSettings LogLevel { get; set; }
}

/// <summary>
/// Impostazioni specifiche per Log4Net.
/// </summary>
public class Log4NetSettings
{
    public required string ConfigFile { get; set; }
    public required LogLevelSettings LogLevel { get; set; }
}

/// <summary>
/// Impostazioni specifiche per Serilog.
/// </summary>
public class SerilogSettings
{
    public List<string> Using { get; set; } = new List<string>();
    public required MinimumLevelSettings MinimumLevel { get; set; }
    public List<WriteToSettings> WriteTo { get; set; } = new List<WriteToSettings>();
}

/// <summary>
/// Impostazioni del livello minimo di logging per Serilog.
/// </summary>
public class MinimumLevelSettings
{
    public required string Default { get; set; }
    public Dictionary<string, string> Override { get; set; } = new Dictionary<string, string>();
}

/// <summary>
/// Impostazioni per la scrittura dei log (es. console, file).
/// </summary>
public class WriteToSettings
{
    public required string Name { get; set; }
    public Dictionary<string, object> Args { get; set; } = new Dictionary<string, object>();
}

/// <summary>
/// Impostazioni dei livelli di logging.
/// </summary>
public class LogLevelSettings
{
    public required string Default { get; set; }
    public required string System { get; set; }
    public required string Microsoft { get; set; }
}