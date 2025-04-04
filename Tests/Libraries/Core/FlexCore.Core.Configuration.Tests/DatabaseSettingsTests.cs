using Xunit;
using FlexCore.Core.Configuration.Models;

namespace FlexCore.Core.Configuration.Tests;

/// <summary>
/// Test suite per la classe DatabaseSettings
/// </summary>
public class DatabaseSettingsTests
{
    /// <summary>
    /// Verifica l'inizializzazione corretta di tutte le proprietà richieste
    /// </summary>
    [Fact]
    public void DatabaseSettings_ShouldInitializeAllRequiredProperties()
    {
        // Arrange & Act
        var dbSettings = new DatabaseSettings
        {
            DefaultProvider = "SQLServer",
            Providers = new List<string> { "SQLServer", "SQLite", "MariaDB" },
            SQLServer = new SQLServerSettings
            {
                ConnectionString = "Server=.;Database=TestDB;",
                EnableRetryOnFailure = true,
                MaxRetryCount = 5,
                MaxRetryDelay = TimeSpan.FromSeconds(30)
            },
            SQLite = new SQLiteSettings
            {
                ConnectionString = "Data Source=test.db;", // ✅ Required
                JournalMode = "WAL", // ✅ Required
                Synchronous = "NORMAL", // ✅ Required
                CacheSize = -2000
            },
            MariaDB = new MariaDBSettings
            {
                ConnectionString = "Server=localhost;Port=3306;Database=test;", // ✅ Required
                Pooling = true // ✅ Required
            }
        };

        // Assert
        Assert.Equal("SQLServer", dbSettings.DefaultProvider);
        Assert.Equal("WAL", dbSettings.SQLite.JournalMode);
        Assert.True(dbSettings.MariaDB.Pooling);
    }
}