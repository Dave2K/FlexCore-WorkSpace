using Xunit;
using FlexCore.Core.Configuration.Models;
using System.Collections.Generic;

namespace FlexCore.Core.Configuration.Tests;

public class AppSettingsTests
{
    [Fact]
    public void AppSettings_ShouldLoadAllSectionsCorrectly()
    {
        var appSettings = new AppSettings
        {
            DatabaseSettings = new DatabaseSettings
            {
                DefaultProvider = "SQLServer",
                Providers = new List<string> { "SQLServer", "SQLite", "MariaDB" },
                SQLServer = new SQLServerSettings
                {
                    ConnectionString = "test_sqlserver",
                    EnableRetryOnFailure = true,
                    MaxRetryCount = 5,
                    MaxRetryDelay = TimeSpan.FromSeconds(30)
                },
                SQLite = new SQLiteSettings
                {
                    ConnectionString = "test_sqlite",
                    JournalMode = "WAL",
                    Synchronous = "Full"
                },
                MariaDB = new MariaDBSettings
                {
                    ConnectionString = "test_mariadb",
                    Pooling = true
                }
            },
            ORMSettings = new ORMSettings
            {
                DefaultProvider = "EFCore",
                Providers = new List<string> { "EFCore", "Dapper", "ADO" },
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
            },
            CacheSettings = new CacheSettings
            {
                DefaultProvider = "Redis",
                Providers = new List<string> { "Redis", "MemoryCache" },
                Redis = new RedisSettings
                {
                    ConnectionString = "test_redis",
                    InstanceName = "test",
                    DefaultDatabase = 0,
                    AbortOnConnectFail = false,
                    ConnectTimeout = 5000,
                    SyncTimeout = 3000
                },
                MemoryCache = new MemoryCacheSettings
                {
                    SizeLimit = 1024,
                    CompactionPercentage = 0.5,
                    ExpirationScanFrequency = TimeSpan.FromMinutes(5)
                }
            },
            Logging = new LoggingSettings
            {
                DefaultProvider = "Serilog",
                Enabled = true,
                Level = "Information",
                Providers = new List<string> { "Console", "Log4Net", "Serilog" },
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
                    ConfigFile = "log4net.config",
                    LogLevel = new LogLevelSettings
                    {
                        Default = "Debug",
                        System = "Error",
                        Microsoft = "Error"
                    }
                },
                Serilog = new SerilogSettings
                {
                    Using = new List<string> { "Console", "File" },
                    MinimumLevel = new MinimumLevelSettings
                    {
                        Default = "Information",
                        Override = new Dictionary<string, string>
                        {
                            { "Microsoft", "Warning" },
                            { "System", "Error" }
                        }
                    },
                    WriteTo = new List<WriteToSettings>
                    {
                        new WriteToSettings
                        {
                            Name = "Console",
                            Args = new Dictionary<string, object>
                            {
                                { "outputTemplate", "{Timestamp} [{Level}] {Message}{NewLine}" }
                            }
                        }
                    }
                }
            },
            SecuritySettings = new SecuritySettings
            {
                DefaultProvider = "JWT",
                Providers = new List<string> { "JWT", "OAuth" },
                JWT = new JwtSettings
                {
                    SecretKey = "test_jwt_key",
                    Issuer = "test_issuer",
                    Audience = "test_audience",
                    ExpiryMinutes = 120
                },
                OAuth = new OAuthSettings
                {
                    Google = new GoogleSettings
                    {
                        ClientId = "test_google_id",
                        ClientSecret = "test_google_secret"
                    },
                    Facebook = new FacebookSettings
                    {
                        ClientId = "test_facebook_id",
                        ClientSecret = "test_facebook_secret"
                    }
                }
            }
        };

        Assert.NotNull(appSettings.DatabaseSettings);
        Assert.NotNull(appSettings.ORMSettings);
        Assert.NotNull(appSettings.CacheSettings);
        Assert.NotNull(appSettings.Logging);
        Assert.NotNull(appSettings.SecuritySettings);
    }
}