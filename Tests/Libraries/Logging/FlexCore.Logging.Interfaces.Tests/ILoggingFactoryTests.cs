// File: Tests/FlexCore.Logging.Interfaces.Tests/ILoggingFactoryTests.cs
using Xunit;
using FlexCore.Logging.Interfaces;

namespace FlexCore.Logging.Interfaces.Tests;

/// <summary>
/// Test per l'interfaccia <see cref="ILoggingFactory"/> 
/// </summary>
public class ILoggingFactoryTests
{
    private class TestProvider : ILoggingProvider
    {
        public void Debug(string message) { }
        public void Info(string message) { }
        public void Warn(string message) { }
        public void Error(string message) { }
        public void Fatal(string message) { }
    }

    private class TestFactory : ILoggingFactory
    {
        public ILoggingProvider CreateProvider(string providerName) => new TestProvider();
    }

    /// <summary>
    /// Verifica che CreateProvider restituisca un'istanza valida
    /// </summary>
    [Fact]
    public void CreateProvider_ShouldReturnProvider()
    {
        var factory = new TestFactory();
        var provider = factory.CreateProvider("test");
        Assert.NotNull(provider);
        Assert.IsAssignableFrom<ILoggingProvider>(provider);
    }
}