using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FlexCore.Core.Configuration.Models;
using FlexCore.Core.Configuration.Extensions;

namespace FlexCore.Core.Configuration.Tests;

/// <summary>
/// Test completi per la copertura al 100% della validazione delle impostazioni
/// </summary>
public class FullCoverageTests
{
    /// <summary>
    /// Verifica il corretto rilevamento di un provider predefinito mancante
    /// </summary>
    [Fact]
    public void ValidateDatabaseSettings_ThrowsForMissingDefaultProvider()
    {
        // Arrange
        var settings = new DatabaseSettings
        {
            DefaultProvider = "",
            Providers = new List<string> { "SQLServer" },
            SQLServer = new SQLServerSettings
            {
                ConnectionString = "dummy_sqlserver",
                MaxRetryDelay = TimeSpan.FromSeconds(30) // ✅ Required
            },
            SQLite = new SQLiteSettings
            {
                ConnectionString = "dummy_sqlite",
                JournalMode = "WAL", // ✅ Required
                Synchronous = "NORMAL" // ✅ Required
            },
            MariaDB = new MariaDBSettings
            {
                ConnectionString = "dummy_mariadb",
                Pooling = true // ✅ Required
            }
        };

        var loggerMock = new Mock<ILogger>();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
            ServiceCollectionExtensions.ValidateDatabaseSettings(settings, loggerMock.Object));

        Assert.Contains("DefaultProvider mancante", ex.Message);
    }

    /// <summary>
    /// Verifica il rilevamento di una connection string mancante per SQL Server
    /// </summary>
    [Fact]
    public void ValidateDatabaseSettings_ThrowsForMissingSQLServerConnectionString()
    {
        // Arrange
        var settings = new DatabaseSettings
        {
            DefaultProvider = "SQLServer",
            Providers = new List<string> { "SQLServer" },
            SQLServer = new SQLServerSettings
            {
                ConnectionString = "", // ❌
                MaxRetryDelay = TimeSpan.Zero // ✅
            },
            SQLite = new SQLiteSettings
            {
                ConnectionString = "dummy",
                JournalMode = "WAL", // ✅
                Synchronous = "NORMAL" // ✅
            },
            MariaDB = new MariaDBSettings
            {
                ConnectionString = "dummy",
                Pooling = true // ✅
            }
        };

        var loggerMock = new Mock<ILogger>();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
            ServiceCollectionExtensions.ValidateDatabaseSettings(settings, loggerMock.Object));

        loggerMock.Verify(log => log.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("SQLServer")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once
        );
    }
}