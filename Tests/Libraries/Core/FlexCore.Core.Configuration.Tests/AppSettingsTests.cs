using Xunit;
using FlexCore.Core.Configuration.Models;

namespace FlexCore.Core.Configuration.Tests;

public class AppSettingsTests
{
    [Fact]
    public void AppSettings_Should_Initialize_Correctly()
    {
        var settings = new AppSettings
        {
            ConnectionStrings = new ConnectionStringsSettings
            {
                DefaultDatabase = "TestDB",
                SQLiteDatabase = "TestSQLite",
                Redis = "TestRedis"
            },
            DatabaseSettings = new DatabaseSettings
            {
                DefaultProvider = "SQLServer",
                SQLite = new SQLiteSettings
                {
                    CacheSize = 100,
                    Synchronous = "Full"
                },
                SQLServer = new SQLServerSettings
                {
                    EnableRetryOnFailure = true,
                    MaxRetryCount = 5,
                    MaxRetryDelay = TimeSpan.FromSeconds(30)
                }
            },
            ORMSettings = new ORMSettings
            {
                DefaultProvider = "EFCore",
                ADO = new ADOSettings
                {
                    ConnectionTimeout = 30
                },
                Dapper = new DapperSettings
                {
                    CommandTimeout = 60
                },
                EFCore = new EFCoreSettings
                {
                    EnableLazyLoading = true,
                    EnableSensitiveDataLogging = false
                }
            },
            CacheSettings = new CacheSettings
            {
                DefaultProvider = "MemoryCache",
                MemoryCache = new MemoryCacheSettings
                {
                    SizeLimit = 1024,
                    CompactionPercentage = 0.2,
                    ExpirationScanFrequency = TimeSpan.FromMinutes(5)
                },
                Redis = new RedisSettings
                {
                    ConnectionString = "localhost",
                    InstanceName = "TestInstance",
                    DefaultDatabase = 0,
                    AbortOnConnectFail = false,
                    ConnectTimeout = 5000,
                    SyncTimeout = 5000
                }
            },
            LoggingSettings = new LoggingSettings
            {
                DefaultProvider = "Console",
                Enabled = true,
                Level = "Information",
                Console = new ConsoleLoggingSettings
                {
                    IncludeScopes = false,
                    LogLevel = new LogLevelSettings
                    {
                        Default = "Warning",
                        System = "Error",
                        Microsoft = "Error"
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
                    MinimumLevel = new MinimumLevelSettings
                    {
                        Default = "Information",
                        Override = new Dictionary<string, string> {
                            {
                                "Microsoft", "Warning" 
                            } 
                        }
                    }
                }
            }
        };

        Assert.NotNull(settings.ConnectionStrings);
        Assert.Equal("TestDB", settings.ConnectionStrings.DefaultDatabase);
        Assert.True(settings.LoggingSettings.Enabled);
    }
}
