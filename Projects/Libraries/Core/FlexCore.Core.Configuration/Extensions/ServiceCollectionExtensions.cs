using Microsoft.Extensions.Logging;
using FlexCore.Core.Configuration.Models;

namespace FlexCore.Core.Configuration.Extensions;

/// <summary>
/// Estensioni per la gestione centralizzata della configurazione
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Valida le impostazioni principali del database
    /// </summary>
    public static void ValidateDatabaseSettings(DatabaseSettings settings, ILogger logger)
    {
        if (string.IsNullOrWhiteSpace(settings.DefaultProvider))
        {
            logger.LogError("DefaultProvider non configurato");
            throw new InvalidOperationException("DatabaseSettings:DefaultProvider mancante");
        }

        switch (settings.DefaultProvider)
        {
            case "SQLServer":
                ValidateSQLServer(settings.SQLServer, logger);
                break;
            case "SQLite":
                ValidateSQLite(settings.SQLite, logger);
                break;
            case "MariaDB":
                ValidateMariaDB(settings.MariaDB, logger);
                break;
            default:
                logger.LogError("Provider non supportato: {Provider}", settings.DefaultProvider);
                throw new InvalidOperationException($"Provider {settings.DefaultProvider} non valido");
        }
    }

    private static void ValidateSQLServer(SQLServerSettings settings, ILogger logger)
    {
        if (string.IsNullOrWhiteSpace(settings.ConnectionString))
            ThrowConnectionError("SQLServer", logger);
    }

    private static void ValidateSQLite(SQLiteSettings settings, ILogger logger)
    {
        if (string.IsNullOrWhiteSpace(settings.ConnectionString))
            ThrowConnectionError("SQLite", logger);
    }

    private static void ValidateMariaDB(MariaDBSettings settings, ILogger logger)
    {
        if (string.IsNullOrWhiteSpace(settings.ConnectionString))
            ThrowConnectionError("MariaDB", logger);
    }

    private static void ThrowConnectionError(string provider, ILogger logger)
    {
        logger.LogError("ConnectionString mancante per {Provider}", provider);
        throw new InvalidOperationException($"Configurare DatabaseSettings:{provider}:ConnectionString");
    }
}