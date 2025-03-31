using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FlexCore.Core.Configuration.Models;

namespace FlexCore.Core.Configuration.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseSettings(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger)
    {
        var settings = configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();
        if (settings == null)
        {
            logger.LogError("Sezione DatabaseSettings mancante in configurazione");
            throw new InvalidOperationException("DatabaseSettings non configurato");
        }

        ValidateDatabaseSettings(settings, logger);
        services.AddSingleton(settings);
        return services;
    }

    private static void ValidateDatabaseSettings(DatabaseSettings settings, ILogger logger)
    {
        if (string.IsNullOrEmpty(settings.DefaultProvider))
        {
            logger.LogError("DefaultProvider non configurato");
            throw new InvalidOperationException("DatabaseSettings:DefaultProvider mancante");
        }

        switch (settings.DefaultProvider)
        {
            case "SQLServer":
                if (string.IsNullOrEmpty(settings.SQLServer.ConnectionString))
                    ThrowConnectionStringException("SQLServer");
                break;
            case "SQLite":
                if (string.IsNullOrEmpty(settings.SQLite.ConnectionString))
                    ThrowConnectionStringException("SQLite");
                break;
            case "MariaDB":
                if (string.IsNullOrEmpty(settings.MariaDB.ConnectionString))
                    ThrowConnectionStringException("MariaDB");
                break;
            default:
                logger.LogError("Provider non supportato: {Provider}", settings.DefaultProvider);
                throw new InvalidOperationException($"Provider database non valido: {settings.DefaultProvider}");
        }
    }

    private static void ThrowConnectionStringException(string provider)
    {
        throw new InvalidOperationException(
            $"ConnectionString mancante per il provider {provider}. " +
            $"Configurare la sezione DatabaseSettings:{provider}:ConnectionString in appsettings.json"
        );
    }
}