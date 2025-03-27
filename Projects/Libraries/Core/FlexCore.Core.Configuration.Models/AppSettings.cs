namespace FlexCore.Core.Configuration.Models;

/// <summary>
/// Impostazioni di configurazione dell'applicazione.
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Impostazioni di connessione al database.
    /// </summary>
    public required ConnectionStringsSettings ConnectionStrings { get; set; }

    /// <summary>
    /// Impostazioni del database.
    /// </summary>
    public required DatabaseSettings DatabaseSettings { get; set; }

    /// <summary>
    /// Impostazioni ORM.
    /// </summary>
    public required ORMSettings ORMSettings { get; set; }

    /// <summary>
    /// Impostazioni della cache.
    /// </summary>
    public required CacheSettings CacheSettings { get; set; }

    /// <summary>
    /// Impostazioni di logging.
    /// </summary>
    public required LoggingSettings Logging { get; set; }
}