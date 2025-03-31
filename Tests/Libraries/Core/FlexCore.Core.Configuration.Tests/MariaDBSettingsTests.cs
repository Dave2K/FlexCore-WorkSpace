using Xunit;
using FlexCore.Core.Configuration.Models;

namespace FlexCore.Core.Configuration.Tests;

public class MariaDBSettingsTests
{
    [Fact]
    public void MariaDBSettings_ShouldMapConnectionStringAndPooling()
    {
        var settings = new MariaDBSettings
        {
            ConnectionString = "Server=localhost;Port=3306;Database=TestDB;User=root;Password=test;",
            Pooling = true
        };

        Assert.Contains("Database=TestDB", settings.ConnectionString);
        Assert.True(settings.Pooling);
    }
}