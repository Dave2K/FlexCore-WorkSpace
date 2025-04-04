using Microsoft.Extensions.Configuration;
using Xunit;
using FlexCore.Core.Configuration.Adapter;
using FlexCore.Core.Configuration.Models;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration.Json;
namespace FlexCore.Core.Configuration.Tests;

public class FullConfigurationIntegrationTests
{
    private string ResourcesFolder = WorkSpace.Generated.WSEnvironment.ResourcesFolder;
    private const string ConfigFile = "appsettings.json";

    [Fact]
    public void FullConfigurationLoad_ShouldMapAllSettingsCorrectly()
    {
        // 1. Verifica esistenza file config
        var configPath = Path.Combine(ResourcesFolder, ConfigFile);
        Assert.True(File.Exists(configPath), $"FILE DI CONFIGURAZIONE MANCANTE: {configPath}");

        // 2. Caricamento configurazione
        var config = new ConfigurationBuilder()
            .SetBasePath(ResourcesFolder)
            .AddJsonFile(ConfigFile, optional: false)
            .Build();

        var adapter = new ConfigurationAdapter(config);

        // 3. Caricamento completo della configurazione
        var appSettings = adapter.GetAppSettings();
        var rawJson = File.ReadAllText(configPath);
        var expectedSettings = JsonSerializer.Deserialize<AppSettings>(
            rawJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // 4. Verifica completa di TUTTE le sezioni
        Assert.NotNull(appSettings);
        Assert.NotNull(expectedSettings);

        VerifyDatabaseSettings(appSettings.DatabaseSettings, expectedSettings.DatabaseSettings);
        VerifyORMSettings(appSettings.ORMSettings, expectedSettings.ORMSettings);
        VerifyCacheSettings(appSettings.CacheSettings, expectedSettings.CacheSettings);
        VerifyLoggingSettings(appSettings.Logging, expectedSettings.Logging);
        VerifySecuritySettings(appSettings.SecuritySettings, expectedSettings.SecuritySettings);
    }

    private void VerifyDatabaseSettings(DatabaseSettings actual, DatabaseSettings expected)
    {
        Assert.Equal(expected.DefaultProvider, actual.DefaultProvider);
        Assert.Equal(expected.Providers, actual.Providers);

        // SQL Server
        Assert.Equal(expected.SQLServer.ConnectionString, actual.SQLServer.ConnectionString);
        Assert.Equal(expected.SQLServer.EnableRetryOnFailure, actual.SQLServer.EnableRetryOnFailure);
        Assert.Equal(expected.SQLServer.MaxRetryCount, actual.SQLServer.MaxRetryCount);
        Assert.Equal(expected.SQLServer.MaxRetryDelay, actual.SQLServer.MaxRetryDelay);

        // SQLite
        Assert.Equal(expected.SQLite.ConnectionString, actual.SQLite.ConnectionString);
        Assert.Equal(expected.SQLite.JournalMode, actual.SQLite.JournalMode);
        Assert.Equal(expected.SQLite.CacheSize, actual.SQLite.CacheSize);

        // MariaDB
        Assert.Equal(expected.MariaDB.ConnectionString, actual.MariaDB.ConnectionString);
        Assert.Equal(expected.MariaDB.Pooling, actual.MariaDB.Pooling);
    }

    private void VerifyORMSettings(ORMSettings actual, ORMSettings expected)
    {
        Assert.Equal(expected.DefaultProvider, actual.DefaultProvider);
        Assert.Equal(expected.Providers, actual.Providers);

        // EFCore
        Assert.Equal(expected.EFCore.EnableLazyLoading, actual.EFCore.EnableLazyLoading);
        Assert.Equal(expected.EFCore.EnableSensitiveDataLogging, actual.EFCore.EnableSensitiveDataLogging);

        // Dapper
        Assert.Equal(expected.Dapper.CommandTimeout, actual.Dapper.CommandTimeout);

        // ADO.NET
        Assert.Equal(expected.ADO.ConnectionTimeout, actual.ADO.ConnectionTimeout);
    }

    private void VerifyCacheSettings(CacheSettings actual, CacheSettings expected)
    {
        Assert.Equal(expected.DefaultProvider, actual.DefaultProvider);
        Assert.Equal(expected.Providers, actual.Providers);

        // Redis
        Assert.Equal(expected.Redis.ConnectionString, actual.Redis.ConnectionString);
        Assert.Equal(expected.Redis.InstanceName, actual.Redis.InstanceName);
        Assert.Equal(expected.Redis.DefaultDatabase, actual.Redis.DefaultDatabase);

        // MemoryCache
        Assert.Equal(expected.MemoryCache.SizeLimit, actual.MemoryCache.SizeLimit);
        Assert.Equal(expected.MemoryCache.CompactionPercentage, actual.MemoryCache.CompactionPercentage);
    }

    private void VerifyLoggingSettings(LoggingSettings actual, LoggingSettings expected)
    {
        Assert.Equal(expected.DefaultProvider, actual.DefaultProvider);
        Assert.Equal(expected.Enabled, actual.Enabled);
        Assert.Equal(expected.Level, actual.Level);

        // Console
        Assert.Equal(expected.Console.IncludeScopes, actual.Console.IncludeScopes);
        Assert.Equal(expected.Console.LogLevel.Default, actual.Console.LogLevel.Default);

        // Serilog
        Assert.Equal(expected.Serilog.MinimumLevel.Default, actual.Serilog.MinimumLevel.Default);
        Assert.Equal(
            expected.Serilog.WriteTo.First().Name,
            actual.Serilog.WriteTo.First().Name);
    }

    private void VerifySecuritySettings(SecuritySettings actual, SecuritySettings expected)
    {
        Assert.Equal(expected.DefaultProvider, actual.DefaultProvider);
        Assert.Equal(expected.Providers, actual.Providers);

        // JWT
        Assert.Equal(expected.JWT.SecretKey, actual.JWT.SecretKey);
        Assert.Equal(expected.JWT.Issuer, actual.JWT.Issuer);
        Assert.Equal(expected.JWT.ExpiryMinutes, actual.JWT.ExpiryMinutes);

        // OAuth
        Assert.Equal(expected.OAuth.Google.ClientId, actual.OAuth.Google.ClientId);
        Assert.Equal(expected.OAuth.Facebook.ClientSecret, actual.OAuth.Facebook.ClientSecret);
    }
}