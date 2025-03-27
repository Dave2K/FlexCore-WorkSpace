// File: Tests/FlexCore.Logging.Factory.Tests/LoggingProviderFactoryTests.cs
using Xunit;
using FlexCore.Logging.Interfaces;
using FlexCore.Logging.Factory;
using FlexCore.Logging.Console;

namespace FlexCore.Logging.Factory.Tests;

/// <summary>
/// Test per la classe <see cref="LoggingProviderFactory"/>
/// </summary>
public class LoggingProviderFactoryTests
{
    /// <summary>
    /// Verifica che il metodo RegisterProvider registri correttamente un provider
    /// </summary>
    [Fact]
    public void RegisterProvider_ShouldStoreProvider()
    {
        // Arrange
        var factory = new LoggingProviderFactory();
        var consoleProvider = new ConsoleLogger();

        // Act
        factory.RegisterProvider("console", () => consoleProvider);
        var provider = factory.CreateProvider("console");

        // Assert
        Assert.Equal(consoleProvider, provider);
    }

    /// <summary>
    /// Verifica che il metodo CreateProvider sollevi eccezione per provider non registrato
    /// </summary>
    [Fact]
    public void CreateProvider_ShouldThrowForUnknownProvider()
    {
        // Arrange
        var factory = new LoggingProviderFactory();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => factory.CreateProvider("unknown"));
    }

    /// <summary>
    /// Verifica che il metodo RegisterProvider sollevi eccezione per factory nulla
    /// </summary>
    [Fact]
    public void RegisterProvider_ShouldThrowForNullFactory()
    {
        // Arrange
        var factory = new LoggingProviderFactory();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => factory.RegisterProvider("test", null!));
    }
}