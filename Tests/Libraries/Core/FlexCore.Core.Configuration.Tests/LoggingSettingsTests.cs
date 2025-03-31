using Xunit;
using FlexCore.Core.Configuration.Models;
using System.Collections.Generic;

namespace FlexCore.Core.Configuration.Tests;

public class LoggingSettingsTests
{
    [Fact]
    public void LoggingSettings_ShouldMapAllProvidersAndLevelsCorrectly()
    {
        var settings = new LoggingSettings
        {
            DefaultProvider = "Serilog",
            Enabled = true, // ✅ Required
            Level = "Information", // ✅ Required
            Providers = new List<string> { "Console", "Log4Net", "Serilog" },
            Console = new ConsoleLoggingSettings
            {
                IncludeScopes = true,
                LogLevel = new LogLevelSettings
                {
                    Default = "Information",
                    System = "Warning", // ✅ Required
                    Microsoft = "Warning" // ✅ Required
                }
            },
            Log4Net = new Log4NetSettings
            {
                ConfigFile = "log4net.test.config",
                LogLevel = new LogLevelSettings
                {
                    Default = "Debug",
                    System = "Error", // ✅ Required
                    Microsoft = "Error" // ✅ Required
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
                    },
                    new WriteToSettings
                    {
                        Name = "File",
                        Args = new Dictionary<string, object>
                        {
                            { "path", "./logs/test-log.txt" },
                            { "rollingInterval", "Day" }
                        }
                    }
                }
            }
        };

        // Test logic...
    }
}