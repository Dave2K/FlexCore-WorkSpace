using Xunit;
using FlexCore.Core.Configuration.Models;

namespace FlexCore.Core.Configuration.Tests;

public class SQLiteSettingsTests
{
    [Fact]
    public void SQLiteSettings_ShouldMapJournalModeAndSynchronous()
    {
        var settings = new SQLiteSettings
        {
            ConnectionString = "Data Source=test.db;Version=3;",
            JournalMode = "WAL",
            Synchronous = "Normal"
        };

        Assert.Equal("WAL", settings.JournalMode);
        Assert.Equal("Normal", settings.Synchronous);
        Assert.Contains("test.db", settings.ConnectionString);
    }
}