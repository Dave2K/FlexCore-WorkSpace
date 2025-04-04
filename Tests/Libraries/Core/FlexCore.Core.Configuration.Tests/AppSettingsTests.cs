using Xunit;
using FlexCore.Core.Configuration.Models;

namespace FlexCore.Core.Configuration.Tests;

/// <summary>
/// Test per la classe principale di configurazione AppSettings
/// </summary>
public class AppSettingsTests
{
    /// <summary>
    /// Verifica che tutte le sezioni di configurazione vengano inizializzate correttamente
    /// </summary>
    [Fact]
    public void AppSettings_ShouldInitializeAllRequiredSections()
    {
        // Arrange & Act
        var appSettings = new AppSettings
        {
            DatabaseSettings = new DatabaseSettings
            {
                DefaultProvider = "SQLServer",
                Providers = new List<string> { "SQLServer" },
                SQLServer = new SQLServerSettings
                {
                    ConnectionString = "test",
                    MaxRetryDelay = TimeSpan.Zero
                },
                SQLite = new SQLiteSettings
                {
                    ConnectionString = "test",
                    JournalMode = "WAL",
                    Synchronous = "NORMAL"
                },
                MariaDB = new MariaDBSettings
                {
                    ConnectionString = "test",
                    Pooling = true
                }
            },
            SecuritySettings = new SecuritySettings
            {
                DefaultProvider = "JWT",
                Providers = new List<string> { "JWT" },
                JWT = new JwtSettings
                {
                    SecretKey = "dummy_key",
                    Issuer = "test",
                    Audience = "test"
                },
                OAuth = new OAuthSettings
                {
                    Google = new GoogleSettings
                    {
                        ClientId = "dummy",
                        ClientSecret = "dummy"
                    },
                    Facebook = new FacebookSettings
                    {
                        ClientId = "dummy",
                        ClientSecret = "dummy"
                    }
                }
            },
            Logging = new LoggingSettings
            {
                DefaultProvider = "Console",
                Providers = new List<string> { "Console" },
                Enabled = true,
                Level = "Information",
                Console = new ConsoleLoggingSettings
                {
                    IncludeScopes = true,
                    LogLevel = new LogLevelSettings
                    {
                        Default = "Information",
                        System = "Warning",
                        Microsoft = "Warning"
                    }
                },
                Log4Net = new Log4NetSettings
                {
                    ConfigFile = "dummy.config",
                    LogLevel = new LogLevelSettings
                    {
                        Default = "Debug",
                        System = "Error",
                        Microsoft = "Error"
                    }
                },
                Serilog = new SerilogSettings
                {
                    Using = new List<string> { "Console" },
                    MinimumLevel = new MinimumLevelSettings
                    {
                        Default = "Information",
                        Override = new Dictionary<string, string>()
                    },
                    WriteTo = new List<WriteToSettings>()
                }
            },
            CacheSettings = new CacheSettings
            {
                DefaultProvider = "MemoryCache",
                Providers = new List<string> { "MemoryCache" },
                Redis = new RedisSettings
                {
                    ConnectionString = "dummy",
                    InstanceName = "test",
                    DefaultDatabase = 0,
                    AbortOnConnectFail = false,
                    ConnectTimeout = 5000,
                    SyncTimeout = 5000
                },
                MemoryCache = new MemoryCacheSettings
                {
                    SizeLimit = 1024,
                    CompactionPercentage = 0.5,
                    ExpirationScanFrequency = TimeSpan.FromMinutes(5)
                }
            },
            ORMSettings = new ORMSettings
            {
                DefaultProvider = "EFCore",
                Providers = new List<string> { "EFCore" },
                EFCore = new EFCoreSettings
                {
                    EnableLazyLoading = false,
                    EnableSensitiveDataLogging = false
                },
                Dapper = new DapperSettings
                {
                    CommandTimeout = 30
                },
                ADO = new ADOSettings
                {
                    ConnectionTimeout = 15
                }
            }
        };

        // Assert
        Assert.NotNull(appSettings.SecuritySettings);
        Assert.NotNull(appSettings.Logging);
        Assert.NotNull(appSettings.CacheSettings);
        Assert.NotNull(appSettings.ORMSettings);
    }
}