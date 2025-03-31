using Xunit;
using FlexCore.Core.Configuration.Models;

namespace FlexCore.Core.Configuration.Tests;

public class ORMSettingsTests
{
    [Fact]
    public void ORMSettings_ShouldMapAllOrmProvidersCorrectly()
    {
        var settings = new ORMSettings
        {
            DefaultProvider = "EFCore",
            Providers = new List<string> { "EFCore", "Dapper", "ADO" },
            EFCore = new EFCoreSettings
            {
                EnableLazyLoading = false,
                EnableSensitiveDataLogging = true
            },
            Dapper = new DapperSettings
            {
                CommandTimeout = 30
            },
            ADO = new ADOSettings
            {
                ConnectionTimeout = 15
            }
        };

        Assert.Equal("EFCore", settings.DefaultProvider);
        Assert.False(settings.EFCore.EnableLazyLoading);
        Assert.True(settings.EFCore.EnableSensitiveDataLogging);
        Assert.Equal(30, settings.Dapper.CommandTimeout);
        Assert.Equal(15, settings.ADO.ConnectionTimeout);
    }
}