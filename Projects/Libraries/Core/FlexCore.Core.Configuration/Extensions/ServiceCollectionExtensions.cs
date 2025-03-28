namespace FlexCore.Core.Configuration.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FlexCore.Core.Configuration.Models;

/// <summary>
/// Estensioni per la configurazione dei servizi.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Aggiunge le impostazioni dell'applicazione ai servizi DI.
    /// </summary>
    /// <param name="services">La collection di servizi DI.</param>
    /// <param name="configuration">La configurazione dell'applicazione.</param>
    /// <param name="logger">Il logger.</param>
    /// <returns>IServiceCollection con le impostazioni registrate.</returns>
    public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
        services.Configure<AppSettings>(options =>
        {
            configuration.Bind(options);
            ValidateAppSettings(options, logger);
        });

        services.Configure<ConnectionStringsSettings>(options =>
        {
            configuration.GetSection("ConnectionStrings").Bind(options);
            ValidateConnectionStrings(options, logger);
        });

        services.Configure<DatabaseSettings>(options =>
        {
            configuration.GetSection("DatabaseSettings").Bind(options);
            ValidateDatabaseSettings(options, logger);
        });

        services.Configure<ORMSettings>(options =>
        {
            configuration.GetSection("ORMSettings").Bind(options);
            ValidateORMSettings(options, logger);
        });

        services.Configure<CacheSettings>(options =>
        {
            configuration.GetSection("CacheSettings").Bind(options);
            ValidateCacheSettings(options, logger);
        });

        services.Configure<LoggingSettings>(options =>
        {
            configuration.GetSection("Logging").Bind(options);
            ValidateLoggingSettings(options, logger);
        });

        return services;
    }

    private static void ValidateAppSettings(AppSettings settings, ILogger logger)
    {
        if (string.IsNullOrEmpty(settings.ConnectionStrings.DefaultDatabase))
            logger.LogWarning("DefaultDatabase non configurato in AppSettings.");
    }

    private static void ValidateConnectionStrings(ConnectionStringsSettings settings, ILogger logger)
    {
        if (string.IsNullOrEmpty(settings.DefaultDatabase))
            logger.LogWarning("DefaultDatabase non configurato in ConnectionStrings.");
    }

    private static void ValidateDatabaseSettings(DatabaseSettings settings, ILogger logger)
    {
        if (string.IsNullOrEmpty(settings.DefaultProvider))
            logger.LogWarning("DefaultProvider non configurato in DatabaseSettings.");
    }

    private static void ValidateORMSettings(ORMSettings settings, ILogger logger)
    {
        if (string.IsNullOrEmpty(settings.DefaultProvider))
            logger.LogWarning("DefaultProvider non configurato in ORMSettings.");
    }

    private static void ValidateCacheSettings(CacheSettings settings, ILogger logger)
    {
        if (string.IsNullOrEmpty(settings.DefaultProvider))
            logger.LogWarning("DefaultProvider non configurato in CacheSettings.");
    }

    private static void ValidateLoggingSettings(LoggingSettings settings, ILogger logger)
    {
        if (!settings.Enabled)
            logger.LogWarning("Logging non abilitato in LoggingSettings.");
    }
}