namespace FlexCore.Core.Configuration.Adapter;

using Microsoft.Extensions.Configuration;
using FlexCore.Core.Configuration.Models;
using FlexCore.Core.Configuration.Interface;

/// <summary>
/// Adattatore di configurazione che fornisce accesso alle configurazioni tramite modelli fortemente tipizzati.
/// </summary>
public class ConfigurationAdapter(IConfiguration configuration) : IAppConfiguration
{
    private readonly IConfiguration _configuration = configuration
        ?? throw new ArgumentNullException(nameof(configuration));

    /// <summary>
    /// Ottiene un valore di configurazione in base alla chiave specificata.
    /// </summary>
    public string? GetValue(string key) => _configuration[key];

    /// <summary>
    /// Ottiene un valore di configurazione fortemente tipizzato.
    /// </summary>
    public T? GetValue<T>(string key) => _configuration.GetValue<T>(key);

    /// <summary>
    /// Ottiene le impostazioni globali dell'applicazione.
    /// </summary>
    public AppSettings GetAppSettings() =>
        _configuration.Get<AppSettings>()
        ?? throw new InvalidOperationException("Impossibile caricare AppSettings");

    /// <summary>
    /// Ottiene le impostazioni di configurazione dei database.
    /// </summary>
    public DatabaseSettings GetDatabaseSettings() =>
        _configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>()
        ?? throw new InvalidOperationException("DatabaseSettings mancante");

    /// <summary>
    /// Ottiene le impostazioni per i provider ORM.
    /// </summary>
    public ORMSettings GetORMSettings() =>
        _configuration.GetSection("ORMSettings").Get<ORMSettings>()
        ?? throw new InvalidOperationException("ORMSettings mancanti");

    /// <summary>
    /// Ottiene le impostazioni per i provider di cache.
    /// </summary>
    public CacheSettings GetCacheSettings() =>
        _configuration.GetSection("CacheSettings").Get<CacheSettings>()
        ?? throw new InvalidOperationException("CacheSettings mancanti");

    /// <summary>
    /// Ottiene le impostazioni di logging.
    /// </summary>
    public LoggingSettings GetLoggingSettings() =>
        _configuration.GetSection("Logging").Get<LoggingSettings>()
        ?? throw new InvalidOperationException("LoggingSettings mancanti");

    /// <summary>
    /// Ottiene le impostazioni di Security.
    /// </summary>
    public SecuritySettings GetSecuritySettings()
    {
        return _configuration.GetSection("SecuritySettings").Get<SecuritySettings>()
            ?? throw new InvalidOperationException("Impossibile caricare le impostazioni di sicurezza.");
    }
}