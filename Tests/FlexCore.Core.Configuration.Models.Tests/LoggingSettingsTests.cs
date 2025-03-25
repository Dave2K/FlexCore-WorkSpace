using Xunit;
using FlexCore.Core.Configuration.Models;

namespace FlexCore.Core.Configuration.Models.Tests;

public class LoggingSettingsTests
{
    [Fact]
    public void LoggingSettings_Should_Initialize_Correctly()
    {
        var settings = new LoggingSettings
        {
            Enabled = true,
            Level = "Information",
            Console = new ConsoleLoggingSettings { IncludeScopes = false, LogLevel = new LogLevelSettings { Default = "Warning", System = "Error", Microsoft = "Error" } },
            Log4Net = new Log4NetSettings { ConfigFile = "log4net.config", LogLevel = new LogLevelSettings { Default = "Debug", System = "Error", Microsoft = "Error" } },
            Serilog = new SerilogSettings { MinimumLevel = new MinimumLevelSettings { Default = "Information", Override = new Dictionary<string, string> { { "Microsoft", "Warning" } } } }
        };

        Assert.True(settings.Enabled);
    }
}
