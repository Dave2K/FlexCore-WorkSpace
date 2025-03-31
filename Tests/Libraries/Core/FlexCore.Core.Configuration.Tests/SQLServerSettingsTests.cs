using Xunit;
using FlexCore.Core.Configuration.Models;

namespace FlexCore.Core.Configuration.Tests;

public class SQLServerSettingsTests
{
    [Fact]
    public void SQLServerSettings_ShouldMapConnectionStringAndRetrySettings()
    {
        var settings = new SQLServerSettings
        {
            ConnectionString = "Server=test;Database=TestDB;User Id=admin;Password=secret;",
            EnableRetryOnFailure = true,
            MaxRetryCount = 3,
            MaxRetryDelay = TimeSpan.FromSeconds(10)
        };

        Assert.Contains("Database=TestDB", settings.ConnectionString);
        Assert.True(settings.EnableRetryOnFailure);
        Assert.Equal(3, settings.MaxRetryCount);
        Assert.Equal(10, settings.MaxRetryDelay.TotalSeconds);
    }
}