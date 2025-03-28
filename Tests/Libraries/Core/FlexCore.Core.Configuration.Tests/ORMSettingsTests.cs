using Xunit;
using FlexCore.Core.Configuration.Models;

namespace FlexCore.Core.Configuration.Models.Tests;

public class ORMSettingsTests
{
    [Fact]
    public void ORMSettings_Should_Initialize_Correctly()
    {
        var settings = new ORMSettings
        {
            DefaultProvider = "EFCore",
            ADO = new ADOSettings { ConnectionTimeout = 30 },
            Dapper = new DapperSettings { CommandTimeout = 60 },
            EFCore = new EFCoreSettings { EnableLazyLoading = true, EnableSensitiveDataLogging = false }
        };

        Assert.Equal("EFCore", settings.DefaultProvider);
    }
}
