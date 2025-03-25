namespace FlexCore.Core.Configuration;

using Microsoft.Extensions.Configuration;
using FlexCore.Core.Configuration.Models;
using System;

/// <summary>
/// Adattatore di configurazione che fornisce accesso alle configurazioni tramite modelli fortemente tipizzati.
/// </summary>
public class ConfigurationAdapter : IAppConfiguration
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="ConfigurationAdapter"/>.
    /// </summary>
    /// <param name="configuration">Istanza di <see cref="IConfiguration"/>.</param>
    public ConfigurationAdapter(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Ottiene un valore di configurazione in base alla chiave specificata.
    /// </summary>
    /// <param name="key">Chiave della configurazione.</param>
    /// <returns>Il valore della configurazione come stringa, oppure null se non presente.</returns>
    public string? GetValue(string key)
    {
        return _configuration[key];
    }

    /// <summary>
    /// Ottiene un valore di configurazione fortemente tipizzato.
    /// </summary>
    /// <typeparam name="T">Tipo del valore da restituire.</typeparam>
    /// <param name="key">Chiave della configurazione.</param>
    /// <returns>Il valore convertito al tipo specificato, oppure il default se non presente.</returns>
    public T? GetValue<T>(string key)
    {
        return _configuration.GetValue<T>(key);
    }

    /// <summary>
    /// Ottiene le impostazioni dell'applicazione.
    /// </summary>
    /// <returns>Le impostazioni dell'applicazione.</returns>
    public AppSettings GetAppSettings()
    {
        return _configuration.Get<AppSettings>() ?? throw new InvalidOperationException("Impossibile caricare le impostazioni dell'applicazione.");
    }

    /// <summary>
    /// Ottiene le impostazioni delle stringhe di connessione.
    /// </summary>
    /// <returns>Le impostazioni delle stringhe di connessione.</returns>
    public ConnectionStringsSettings GetConnectionStrings()
    {
        return _configuration.GetSection("ConnectionStrings").Get<ConnectionStringsSettings>()
            ?? throw new InvalidOperationException("Impossibile caricare le impostazioni delle stringhe di connessione.");
    }

    /// <summary>
    /// Ottiene le impostazioni del database.
    /// </summary>
    /// <returns>Le impostazioni del database.</returns>
    public DatabaseSettings GetDatabaseSettings()
    {
        return _configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>()
            ?? throw new InvalidOperationException("Impossibile caricare le impostazioni del database.");
    }

    /// <summary>
    /// Ottiene le impostazioni ORM.
    /// </summary>
    /// <returns>Le impostazioni ORM.</returns>
    public ORMSettings GetORMSettings()
    {
        return _configuration.GetSection("ORMSettings").Get<ORMSettings>()
            ?? throw new InvalidOperationException("Impossibile caricare le impostazioni ORM.");
    }

    /// <summary>
    /// Ottiene le impostazioni della cache.
    /// </summary>
    /// <returns>Le impostazioni della cache.</returns>
    public CacheSettings GetCacheSettings()
    {
        return _configuration.GetSection("CacheSettings").Get<CacheSettings>()
            ?? throw new InvalidOperationException("Impossibile caricare le impostazioni della cache.");
    }

    /// <summary>
    /// Ottiene le impostazioni di logging.
    /// </summary>
    /// <returns>Le impostazioni di logging.</returns>
    public LoggingSettings GetLoggingSettings()
    {
        return _configuration.GetSection("Logging").Get<LoggingSettings>()
            ?? throw new InvalidOperationException("Impossibile caricare le impostazioni di logging.");
    }
}