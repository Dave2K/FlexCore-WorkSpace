using FlexCore.Core.Configuration.Adapter;
using FlexCore.Core.Configuration.Interface;
using FlexCore.Core.Configuration.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkSpace.Generated;

namespace FlexCore.Configuration.Demo;

class Program
{
    static void Main()
    {
        string resourcesFolder = Enviroment.ResourcesFolder;
        Console.WriteLine($"Cartella di lavoro: {resourcesFolder}");

        string configPath = Path.Combine(resourcesFolder, "appsettings.json");
        if (!File.Exists(configPath))
        {
            Console.WriteLine($"Errore: Il file di configurazione non esiste in {configPath}");
            return;
        }

        var config = new ConfigurationBuilder()
            .SetBasePath(resourcesFolder)
            .AddJsonFile("appsettings.json", optional: false)  //, reloadOnChange: true
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(config);
        services.AddScoped<IAppConfiguration, ConfigurationAdapter>();

        var provider = services.BuildServiceProvider();
        var appConfig = provider.GetRequiredService<IAppConfiguration>();

        try
        {
            // Carica tutte le impostazioni
            var dbSettings = appConfig.GetDatabaseSettings();
            var ormSettings = appConfig.GetORMSettings();
            var cacheSettings = appConfig.GetCacheSettings();
            var loggingSettings = appConfig.GetLoggingSettings();
            var securitySettings = appConfig.GetSecuritySettings(); // ✅ Ora funziona

            // Stampa tutte le impostazioni
            PrintDatabaseSettings(dbSettings);
            PrintORMSettings(ormSettings);
            PrintCacheSettings(cacheSettings);
            PrintLoggingSettings(loggingSettings);
            PrintSecuritySettings(securitySettings);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il caricamento della configurazione: {ex.Message}");
        }
    }

    private static void PrintDatabaseSettings(DatabaseSettings settings)
    {
        Console.WriteLine("\n=== DATABASE SETTINGS ===");
        Console.WriteLine($"Default Provider: {settings.DefaultProvider}");
        Console.WriteLine($"Supported Providers: {string.Join(", ", settings.Providers)}");

        Console.WriteLine("\nSQL Server:");
        Console.WriteLine($"- ConnectionString: {settings.SQLServer.ConnectionString}");
        Console.WriteLine($"- Retry on Failure: {settings.SQLServer.EnableRetryOnFailure}");
        Console.WriteLine($"- Max Retries: {settings.SQLServer.MaxRetryCount}");
        Console.WriteLine($"- Max Retry Delay: {settings.SQLServer.MaxRetryDelay}");

        Console.WriteLine("\nSQLite:");
        Console.WriteLine($"- ConnectionString: {settings.SQLite.ConnectionString}");
        Console.WriteLine($"- Journal Mode: {settings.SQLite.JournalMode}");

        Console.WriteLine("\nMariaDB:");
        Console.WriteLine($"- ConnectionString: {settings.MariaDB.ConnectionString}");
        Console.WriteLine($"- Pooling: {settings.MariaDB.Pooling}");
    }

    private static void PrintORMSettings(ORMSettings settings)
    {
        Console.WriteLine("\n=== ORM SETTINGS ===");
        Console.WriteLine($"Default Provider: {settings.DefaultProvider}");
        Console.WriteLine($"Supported Providers: {string.Join(", ", settings.Providers)}");

        Console.WriteLine("\nEF Core:");
        Console.WriteLine($"- Lazy Loading: {settings.EFCore.EnableLazyLoading}");
        Console.WriteLine($"- Sensitive Data Logging: {settings.EFCore.EnableSensitiveDataLogging}");

        Console.WriteLine("\nDapper:");
        Console.WriteLine($"- Command Timeout: {settings.Dapper.CommandTimeout}s");

        Console.WriteLine("\nADO.NET:");
        Console.WriteLine($"- Connection Timeout: {settings.ADO.ConnectionTimeout}s");
    }

    private static void PrintCacheSettings(CacheSettings settings)
    {
        Console.WriteLine("\n=== CACHE SETTINGS ===");
        Console.WriteLine($"Default Provider: {settings.DefaultProvider}");
        Console.WriteLine($"Supported Providers: {string.Join(", ", settings.Providers)}");

        Console.WriteLine("\nRedis:");
        Console.WriteLine($"- ConnectionString: {settings.Redis.ConnectionString}");
        Console.WriteLine($"- Instance Name: {settings.Redis.InstanceName}");
        Console.WriteLine($"- Connect Timeout: {settings.Redis.ConnectTimeout}ms");

        Console.WriteLine("\nMemory Cache:");
        Console.WriteLine($"- Size Limit: {settings.MemoryCache.SizeLimit}");
        Console.WriteLine($"- Compaction %: {settings.MemoryCache.CompactionPercentage:P0}");
        Console.WriteLine($"- Expiration Scan: {settings.MemoryCache.ExpirationScanFrequency}");
    }

    private static void PrintLoggingSettings(LoggingSettings settings)
    {
        Console.WriteLine("\n=== LOGGING SETTINGS ===");
        Console.WriteLine($"Default Provider: {settings.DefaultProvider}");
        Console.WriteLine($"Supported Providers: {string.Join(", ", settings.Providers)}");

        Console.WriteLine("\nConsole:");
        Console.WriteLine($"- Include Scopes: {settings.Console.IncludeScopes}");
        Console.WriteLine($"- Log Level Default: {settings.Console.LogLevel.Default}");

        Console.WriteLine("\nLog4Net:");
        Console.WriteLine($"- Config File: {settings.Log4Net.ConfigFile}");
        Console.WriteLine($"- Log Level Default: {settings.Log4Net.LogLevel.Default}");

        Console.WriteLine("\nSerilog:");
        Console.WriteLine("- Minimum Levels:");
        Console.WriteLine($"  Default: {settings.Serilog.MinimumLevel.Default}");
        Console.WriteLine($"  Overrides: Microsoft={settings.Serilog.MinimumLevel.Override["Microsoft"]}, System={settings.Serilog.MinimumLevel.Override["System"]}");
        Console.WriteLine("- Write To:");
        foreach (var sink in settings.Serilog.WriteTo)
        {
            Console.WriteLine($"  {sink.Name}: {string.Join(", ", sink.Args)}");
        }
    }

    private static void PrintSecuritySettings(SecuritySettings settings)
    {
        Console.WriteLine("\n=== SECURITY SETTINGS ===");
        Console.WriteLine($"Default Provider: {settings.DefaultProvider}");
        Console.WriteLine($"Supported Providers: {string.Join(", ", settings.Providers)}");

        Console.WriteLine("\nJWT:");
        Console.WriteLine($"- Secret Key: {new string('*', settings.JWT.SecretKey.Length)}");
        Console.WriteLine($"- Issuer: {settings.JWT.Issuer}");
        Console.WriteLine($"- Audience: {settings.JWT.Audience}");
        Console.WriteLine($"- Expiry: {settings.JWT.ExpiryMinutes} minuti");

        Console.WriteLine("\nOAuth - Google:");
        Console.WriteLine($"- Client ID: {settings.OAuth.Google.ClientId}");
        Console.WriteLine($"- Client Secret: {new string('*', settings.OAuth.Google.ClientSecret.Length)}");

        Console.WriteLine("\nOAuth - Facebook:");
        Console.WriteLine($"- Client ID: {settings.OAuth.Facebook.ClientId}");
        Console.WriteLine($"- Client Secret: {new string('*', settings.OAuth.Facebook.ClientSecret.Length)}");
    }
}