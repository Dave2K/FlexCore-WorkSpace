namespace FlexCore.Core.Configuration.Models;

/// <summary>
/// Modello principale delle impostazioni dell'applicazione (allineato al JSON).
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Impostazioni globali per la gestione dei database.
    /// </summary>
    public required DatabaseSettings DatabaseSettings { get; set; }

    /// <summary>
    /// Impostazioni per i provider ORM (EFCore, Dapper, ADO.NET).
    /// </summary>
    public required ORMSettings ORMSettings { get; set; }

    /// <summary>
    /// Impostazioni per i provider di cache (Redis, MemoryCache).
    /// </summary>
    public required CacheSettings CacheSettings { get; set; }

    /// <summary>
    /// Impostazioni di logging (Console, Log4Net, Serilog).
    /// </summary>
    public required LoggingSettings Logging { get; set; }

    /// <summary>
    /// Impostazioni di sicurezza (JWT, OAuth).
    /// </summary>
    public required SecuritySettings SecuritySettings { get; set; }
}