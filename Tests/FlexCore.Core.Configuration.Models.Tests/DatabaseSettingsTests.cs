using Xunit;
using FlexCore.Core.Configuration.Models;

namespace FlexCore.Core.Configuration.Models.Tests;

public class DatabaseSettingsTests
{
    [Fact]
    public void DatabaseSettings_Should_Initialize_Correctly()
    {
        var settings = new DatabaseSettings
        {
            DefaultProvider = "SQLServer",
            SQLite = new SQLiteSettings { CacheSize = 100, Synchronous = "Full" },
            SQLServer = new SQLServerSettings { EnableRetryOnFailure = true, MaxRetryCount = 5, MaxRetryDelay = TimeSpan.FromSeconds(30) }
        };

        Assert.Equal("SQLServer", settings.DefaultProvider);
    }
}
