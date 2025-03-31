using FlexCore.Core.Configuration.Models;

namespace FlexCore.Core.Configuration.Interface;

/// <summary>
/// Interfaccia per l'accesso alle configurazioni dell'applicazione.
/// </summary>
public interface IAppConfiguration
{
    /// <summary>
    /// Ottiene un valore di configurazione in base alla chiave specificata.
    /// </summary>
    /// <param name="key">Chiave della configurazione.</param>
    /// <returns>Il valore della configurazione come stringa, oppure null se non presente.</returns>
    string? GetValue(string key);

    /// <summary>
    /// Ottiene un valore di configurazione fortemente tipizzato.
    /// </summary>
    /// <typeparam name="T">Tipo del valore da restituire.</typeparam>
    /// <param name="key">Chiave della configurazione.</param>
    /// <returns>Il valore convertito al tipo specificato, oppure il default se non presente.</returns>
    T? GetValue<T>(string key);

    /// <summary>
    /// Ottiene le impostazioni globali dell'applicazione.
    /// </summary>
    /// <returns>Le impostazioni dell'applicazione.</returns>
    AppSettings GetAppSettings();

    /// <summary>
    /// Ottiene le impostazioni di configurazione dei database.
    /// </summary>
    /// <returns>Provider, connessioni e parametri dei database.</returns>
    DatabaseSettings GetDatabaseSettings();

    /// <summary>
    /// Ottiene le impostazioni per i provider ORM (EFCore, Dapper, ADO.NET).
    /// </summary>
    /// <returns>Le impostazioni ORM.</returns>
    ORMSettings GetORMSettings();

    /// <summary>
    /// Ottiene le impostazioni per i provider di cache (Redis, MemoryCache).
    /// </summary>
    /// <returns>Le impostazioni della cache.</returns>
    CacheSettings GetCacheSettings();

    /// <summary>
    /// Ottiene le impostazioni di logging (Console, Log4Net, Serilog).
    /// </summary>
    /// <returns>Le impostazioni di logging.</returns>
    LoggingSettings GetLoggingSettings();

    /// <summary>
    /// Ottiene le impostazioni di sicurezza (JWT, OAuth).
    /// </summary>
    SecuritySettings GetSecuritySettings();
}