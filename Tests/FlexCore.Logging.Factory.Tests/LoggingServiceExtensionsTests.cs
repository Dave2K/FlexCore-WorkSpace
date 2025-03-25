// File: Tests/FlexCore.Logging.Factory.Tests/LoggingServiceExtensionsTests.cs
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using FlexCore.Logging.Interfaces;
using FlexCore.Logging.Factory;

namespace FlexCore.Logging.Factory.Tests;

/// <summary>
/// Test per le estensioni DI <see cref="LoggingServiceExtensions"/>
/// </summary>
public class LoggingServiceExtensionsTests
{
    private class TestProvider : ILoggingProvider
    {
        public void Debug(string message) { }
        public void Info(string message) { }
        public void Warn(string message) { }
        public void Error(string message) { }
        public void Fatal(string message) { }
    }

    /// <summary>
    /// Verifica che il metodo AddLoggingProvider registri i servizi nel container DI
    /// </summary>
    [Fact]
    public void AddLoggingProvider_ShouldRegisterServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var factory = new LoggingProviderFactory();
        factory.RegisterProvider("test", () => new TestProvider());

        // Act
        services.AddSingleton<ILoggingFactory>(factory);
        services.AddSingleton<ILoggingProvider>(factory.CreateProvider("test"));

        // Assert
        var provider = services.BuildServiceProvider();
        Assert.NotNull(provider.GetService<ILoggingFactory>());
        Assert.NotNull(provider.GetService<ILoggingProvider>());
    }
}