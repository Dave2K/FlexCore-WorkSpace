using Xunit;
using System.Collections.Generic;
using FlexCore.Core.Configuration.Models;

namespace FlexCore.Logging.Core.Tests
{
    /// <summary>
    /// Test per la classe LoggingSettings
    /// </summary>
    public class LoggingSettingsTests
    {
        /// <summary>
        /// Verifica che i valori predefiniti delle impostazioni di logging siano corretti.
        /// </summary>
        [Fact]
        public void DefaultSettings_ShouldHaveExpectedValues()
        {
            var settings = new LoggingSettings
            {
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
                        Override = new Dictionary<string, string> { { "Microsoft", "Warning" } }
                    }
                }
            };

            Assert.True(settings.Enabled);
            Assert.Equal("Information", settings.Level);
            Assert.NotNull(settings.Console);
            Assert.NotNull(settings.Log4Net);
            Assert.NotNull(settings.Serilog);
        }

        /// <summary>
        /// Verifica che sia possibile modificare le impostazioni di logging.
        /// </summary>
        [Fact]
        public void CanModifyLoggingSettings()
        {
            var settings = new LoggingSettings
            {
                Enabled = false,
                Level = "Debug",
                Console = new ConsoleLoggingSettings
                {
                    IncludeScopes = true,
                    LogLevel = new LogLevelSettings
                    {
                        Default = "Error",
                        System = "Warning",
                        Microsoft = "Warning"
                    }
                },
                Log4Net = new Log4NetSettings
                {
                    ConfigFile = "custom_log4net.config",
                    LogLevel = new LogLevelSettings
                    {
                        Default = "Info",
                        System = "Debug",
                        Microsoft = "Debug"
                    }
                },
                Serilog = new SerilogSettings
                {
                    MinimumLevel = new MinimumLevelSettings
                    {
                        Default = "Debug",
                        Override = new Dictionary<string, string> { { "Microsoft", "Error" } }
                    }
                }
            };

            Assert.False(settings.Enabled);
            Assert.Equal("Debug", settings.Level);
            Assert.True(settings.Console.IncludeScopes);
            Assert.Equal("Error", settings.Console.LogLevel.Default);
            Assert.Equal("Debug", settings.Log4Net.LogLevel.System);
            Assert.Equal("Debug", settings.Serilog.MinimumLevel.Default);
        }
    }
}
