using Microsoft.Extensions.Configuration;
using Xunit;
using FlexCore.Core.Configuration.Adapter;
using FlexCore.Core.Configuration.Models;
using System;
using System.Collections.Generic;

namespace FlexCore.Core.Configuration.Tests
{
    /// <summary>
    /// Classe di test per verificare il corretto funzionamento di <see cref="ConfigurationAdapter"/>.
    /// </summary>
    public class ConfigurationAdapterTests
    {
        /// <summary>
        /// Verifica che le impostazioni del database vengano caricate correttamente.
        /// </summary>
        [Fact]
        public void GetDatabaseSettings_ShouldReturnValidSettings()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> // ✅ Aggiunto string?
            {
                ["DatabaseSettings:DefaultProvider"] = "SQLServer",
                ["DatabaseSettings:Providers:0"] = "SQLServer",
                ["DatabaseSettings:Providers:1"] = "SQLite",
                ["DatabaseSettings:SQLServer:EnableRetryOnFailure"] = "true",
                ["DatabaseSettings:SQLServer:MaxRetryCount"] = "5",
                ["DatabaseSettings:SQLServer:MaxRetryDelay"] = "00:00:30",
                ["DatabaseSettings:SQLite:CacheSize"] = "1000",
                ["DatabaseSettings:SQLite:Synchronous"] = "Normal"
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings) // ✅ Ora compatibile
                .Build();

            var adapter = new ConfigurationAdapter(configuration);

            // Act
            DatabaseSettings result = adapter.GetDatabaseSettings();

            // Assert
            Assert.Equal("SQLServer", result.DefaultProvider);
            Assert.Equal(["SQLServer", "SQLite"], result.Providers);
            Assert.True(result.SQLServer.EnableRetryOnFailure);
            Assert.Equal(5, result.SQLServer.MaxRetryCount);
            Assert.Equal(TimeSpan.FromSeconds(30), result.SQLServer.MaxRetryDelay);
            Assert.Equal(1000, result.SQLite.CacheSize);
            Assert.Equal("Normal", result.SQLite.Synchronous);
        }

        /// <summary>
        /// Verifica che le impostazioni ORM vengano caricate correttamente.
        /// </summary>
        [Fact]
        public void GetORMSettings_ShouldReturnValidSettings()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> // ✅ Aggiunto string?
            {
                ["ORMSettings:DefaultProvider"] = "EFCore",
                ["ORMSettings:Providers:0"] = "EFCore",
                ["ORMSettings:Providers:1"] = "Dapper",
                ["ORMSettings:Providers:2"] = "ADO",
                ["ORMSettings:EFCore:EnableLazyLoading"] = "true",
                ["ORMSettings:EFCore:EnableSensitiveDataLogging"] = "false",
                ["ORMSettings:Dapper:CommandTimeout"] = "30",
                ["ORMSettings:ADO:ConnectionTimeout"] = "15"
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings) // ✅ Ora compatibile
                .Build();

            var adapter = new ConfigurationAdapter(configuration);

            // Act
            ORMSettings result = adapter.GetORMSettings();

            // Assert
            Assert.Equal("EFCore", result.DefaultProvider);
            Assert.Equal(["EFCore", "Dapper", "ADO"], result.Providers);
            Assert.True(result.EFCore.EnableLazyLoading);
            Assert.False(result.EFCore.EnableSensitiveDataLogging);
            Assert.Equal(30, result.Dapper.CommandTimeout);
            Assert.Equal(15, result.ADO.ConnectionTimeout);
        }

        /// <summary>
        /// Verifica che le impostazioni della cache vengano caricate correttamente.
        /// </summary>
        [Fact]
        public void GetCacheSettings_ShouldReturnValidSettings()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> // ✅ Aggiunto string?
            {
                ["CacheSettings:DefaultProvider"] = "MemoryCache",
                ["CacheSettings:Providers:0"] = "MemoryCache",
                ["CacheSettings:Providers:1"] = "Redis",
                ["CacheSettings:MemoryCache:SizeLimit"] = "1000",
                ["CacheSettings:MemoryCache:CompactionPercentage"] = "0.5",
                ["CacheSettings:MemoryCache:ExpirationScanFrequency"] = "00:01:00",
                ["CacheSettings:Redis:ConnectionString"] = "localhost:6379",
                ["CacheSettings:Redis:InstanceName"] = "RedisInstance",
                ["CacheSettings:Redis:DefaultDatabase"] = "0",
                ["CacheSettings:Redis:AbortOnConnectFail"] = "false",
                ["CacheSettings:Redis:ConnectTimeout"] = "5000",
                ["CacheSettings:Redis:SyncTimeout"] = "1000"
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings) // ✅ Ora compatibile
                .Build();

            var adapter = new ConfigurationAdapter(configuration);

            // Act
            CacheSettings result = adapter.GetCacheSettings();

            // Assert
            Assert.Equal("MemoryCache", result.DefaultProvider);
            Assert.Equal(["MemoryCache", "Redis"], result.Providers);
            Assert.Equal(1000, result.MemoryCache.SizeLimit);
            Assert.Equal(0.5, result.MemoryCache.CompactionPercentage);
            Assert.Equal(TimeSpan.FromMinutes(1), result.MemoryCache.ExpirationScanFrequency);
            Assert.Equal("localhost:6379", result.Redis.ConnectionString);
            Assert.Equal("RedisInstance", result.Redis.InstanceName);
        }

        /// <summary>
        /// Verifica che le impostazioni di logging vengano caricate correttamente.
        /// </summary>
        [Fact]
        public void GetLoggingSettings_ShouldReturnValidSettings()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> // ✅ Aggiunto string?
            {
                ["Logging:Enabled"] = "true",
                ["Logging:Level"] = "Information",
                ["Logging:Providers:0"] = "Console",
                ["Logging:Providers:1"] = "Log4Net",
                ["Logging:Console:IncludeScopes"] = "true",
                ["Logging:Console:LogLevel:Default"] = "Information",
                ["Logging:Console:LogLevel:System"] = "Warning",
                ["Logging:Console:LogLevel:Microsoft"] = "Warning",
                ["Logging:Log4Net:ConfigFile"] = "log4net.config",
                ["Logging:Log4Net:LogLevel:Default"] = "Warning",
                ["Logging:Log4Net:LogLevel:System"] = "Error",
                ["Logging:Log4Net:LogLevel:Microsoft"] = "Error",
                ["Logging:Serilog:MinimumLevel:Default"] = "Information",
                ["Logging:Serilog:WriteTo:0:Name"] = "Console"
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings) // ✅ Ora compatibile
                .Build();

            var adapter = new ConfigurationAdapter(configuration);

            // Act
            LoggingSettings result = adapter.GetLoggingSettings();

            // Assert
            Assert.True(result.Enabled);
            Assert.Equal("Information", result.Level);
            Assert.Equal(["Console", "Log4Net"], result.Providers);
            Assert.True(result.Console.IncludeScopes);
            Assert.Equal("log4net.config", result.Log4Net.ConfigFile);
            Assert.Equal("Information", result.Serilog.MinimumLevel.Default);
        }

        /// <summary>
        /// Verifica il caricamento completo delle impostazioni dell'applicazione.
        /// </summary>
        [Fact]
        public void GetAppSettings_ShouldReturnCompleteConfiguration()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> // ✅ Aggiunto string?
            {
                // ConnectionStrings
                ["ConnectionStrings:DefaultDatabase"] = "Server=test;Database=TestDB",
                ["ConnectionStrings:SQLiteDatabase"] = "Data Source=test.db",
                ["ConnectionStrings:Redis"] = "localhost:6379",

                // DatabaseSettings
                ["DatabaseSettings:DefaultProvider"] = "SQLServer",
                ["DatabaseSettings:Providers:0"] = "SQLServer",
                ["DatabaseSettings:Providers:1"] = "SQLite",
                ["DatabaseSettings:SQLServer:EnableRetryOnFailure"] = "true",
                ["DatabaseSettings:SQLServer:MaxRetryCount"] = "5",
                ["DatabaseSettings:SQLServer:MaxRetryDelay"] = "00:00:30",
                ["DatabaseSettings:SQLite:CacheSize"] = "1000",
                ["DatabaseSettings:SQLite:Synchronous"] = "Normal",

                // ORMSettings
                ["ORMSettings:DefaultProvider"] = "EFCore",
                ["ORMSettings:Providers:0"] = "EFCore",
                ["ORMSettings:Providers:1"] = "Dapper",
                ["ORMSettings:Providers:2"] = "ADO",
                ["ORMSettings:EFCore:EnableLazyLoading"] = "true",
                ["ORMSettings:EFCore:EnableSensitiveDataLogging"] = "false",
                ["ORMSettings:Dapper:CommandTimeout"] = "30",
                ["ORMSettings:ADO:ConnectionTimeout"] = "15",

                // CacheSettings
                ["CacheSettings:DefaultProvider"] = "MemoryCache",
                ["CacheSettings:Providers:0"] = "MemoryCache",
                ["CacheSettings:Providers:1"] = "Redis",
                ["CacheSettings:MemoryCache:SizeLimit"] = "1000",
                ["CacheSettings:MemoryCache:CompactionPercentage"] = "0.5",
                ["CacheSettings:MemoryCache:ExpirationScanFrequency"] = "00:01:00",
                ["CacheSettings:Redis:ConnectionString"] = "localhost:6379",
                ["CacheSettings:Redis:InstanceName"] = "RedisInstance",
                ["CacheSettings:Redis:DefaultDatabase"] = "0",
                ["CacheSettings:Redis:AbortOnConnectFail"] = "false",
                ["CacheSettings:Redis:ConnectTimeout"] = "5000",
                ["CacheSettings:Redis:SyncTimeout"] = "1000",

                // LoggingSettings
                ["Logging:Enabled"] = "true",
                ["Logging:Level"] = "Information",
                ["Logging:Providers:0"] = "Console",
                ["Logging:Providers:1"] = "Log4Net",
                ["Logging:Providers:2"] = "Serilog",
                ["Logging:Console:IncludeScopes"] = "true",
                ["Logging:Console:LogLevel:Default"] = "Information",
                ["Logging:Console:LogLevel:System"] = "Warning",
                ["Logging:Console:LogLevel:Microsoft"] = "Warning",
                ["Logging:Log4Net:ConfigFile"] = "log4net.config",
                ["Logging:Log4Net:LogLevel:Default"] = "Warning",
                ["Logging:Log4Net:LogLevel:System"] = "Error",
                ["Logging:Log4Net:LogLevel:Microsoft"] = "Error",
                ["Logging:Serilog:Using:0"] = "Console",
                ["Logging:Serilog:Using:1"] = "File",
                ["Logging:Serilog:MinimumLevel:Default"] = "Information",
                ["Logging:Serilog:WriteTo:0:Name"] = "Console"
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings) // ✅ Ora compatibile
                .Build();

            var adapter = new ConfigurationAdapter(configuration);

            // Act
            AppSettings result = adapter.GetAppSettings();

            // Assert
            Assert.NotNull(result.ConnectionStrings);
            Assert.NotNull(result.DatabaseSettings);
            Assert.NotNull(result.ORMSettings);
            Assert.NotNull(result.CacheSettings);
            Assert.NotNull(result.LoggingSettings);
        }
    }
}