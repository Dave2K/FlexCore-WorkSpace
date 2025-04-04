using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FlexCore.Core.Configuration.Models;
using FlexCore.Core.Configuration.Extensions;

namespace FlexCore.Core.Configuration.Tests;

/// <summary>
/// Test suite per le estensioni di configurazione del database
/// </summary>
public class ServiceCollectionExtensionsTests
{
    /// <summary>
    /// Verifica il corretto rilevamento di un provider non supportato
    /// </summary>
    [Fact]
    public void ValidateDatabaseSettings_ThrowsForUnsupportedProvider()
    {
        // Arrange
        var invalidSettings = new DatabaseSettings
        {
            DefaultProvider = "Oracle",
            Providers = new List<string> { "SQLServer", "SQLite" },
            SQLServer = new SQLServerSettings
            {
                ConnectionString = "valid_conn",
                MaxRetryDelay = TimeSpan.Zero
            },
            SQLite = new SQLiteSettings
            {
                ConnectionString = "valid_conn",
                JournalMode = "WAL",
                Synchronous = "NORMAL"
            },
            MariaDB = new MariaDBSettings
            {
                ConnectionString = "valid_conn",
                Pooling = true
            }
        };

        var loggerMock = new Mock<ILogger>();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
            ServiceCollectionExtensions.ValidateDatabaseSettings(invalidSettings, loggerMock.Object));

        // Verify
        loggerMock.Verify(log => log.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Oracle")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    /// <summary>
    /// Verifica il rilevamento di una connection string mancante per SQLite
    /// </summary>
    [Fact]
    public void ValidateDatabaseSettings_ThrowsForMissingSQLiteConnectionString()
    {
        // Arrange
        var settings = new DatabaseSettings
        {
            DefaultProvider = "SQLite",
            Providers = new List<string> { "SQLite" },
            SQLite = new SQLiteSettings
            {
                ConnectionString = "", // ❌
                JournalMode = "WAL",
                Synchronous = "NORMAL"
            },
            SQLServer = new SQLServerSettings
            {
                ConnectionString = "dummy",
                MaxRetryDelay = TimeSpan.Zero
            },
            MariaDB = new MariaDBSettings
            {
                ConnectionString = "dummy",
                Pooling = true
            }
        };

        var loggerMock = new Mock<ILogger>();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
            ServiceCollectionExtensions.ValidateDatabaseSettings(settings, loggerMock.Object));

        // Verify
        loggerMock.Verify(log => log.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("SQLite")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}