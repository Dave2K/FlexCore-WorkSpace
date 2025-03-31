using Xunit;
using FlexCore.Core.Configuration.Models;

namespace FlexCore.Core.Configuration.Tests;

public class DatabaseSettingsTests
{
    [Fact]
    public void DatabaseSettings_ShouldMapCorrectly_FromAppSettingsJson()
    {
        // Configurazione di esempio (simile alla sezione "DatabaseSettings" in appsettings.json)
        var dbSettings = new DatabaseSettings
        {
            DefaultProvider = "SQLServer",
            Providers = new List<string> { "SQLServer", "SQLite", "MariaDB" },
            SQLServer = new SQLServerSettings
            {
                ConnectionString = "Server=.;Database=MasterDB;User Id=sa;Password=YourStrong!Pass123;",
                EnableRetryOnFailure = true,
                MaxRetryCount = 5,
                MaxRetryDelay = TimeSpan.FromSeconds(45)
            },
            SQLite = new SQLiteSettings
            {
                ConnectionString = "Data Source=./database.db;Version=3;",
                JournalMode = "WAL",
                CacheSize = -2000,
                Synchronous = "Full"
            },
            MariaDB = new MariaDBSettings
            {
                ConnectionString = "Server=localhost;Port=3306;Database=FlexDB;User=root;Password=mariaPassword;",
                Pooling = true
            }
        };

        // Assert: Verifica il mapping delle proprietà principali
        Assert.Equal("SQLServer", dbSettings.DefaultProvider);
        Assert.Equal(["SQLServer", "SQLite", "MariaDB"], dbSettings.Providers);

        // Assert: Verifica le impostazioni di SQLServer
        Assert.True(dbSettings.SQLServer.EnableRetryOnFailure);
        Assert.Equal(5, dbSettings.SQLServer.MaxRetryCount);
        Assert.Equal(TimeSpan.FromSeconds(45), dbSettings.SQLServer.MaxRetryDelay);

        // Assert: Verifica le impostazioni di SQLite
        Assert.Equal("WAL", dbSettings.SQLite.JournalMode);
        Assert.Equal(-2000, dbSettings.SQLite.CacheSize);

        // Assert: Verifica le impostazioni di MariaDB
        Assert.True(dbSettings.MariaDB.Pooling);
    }
}