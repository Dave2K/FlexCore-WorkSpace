// File: Tests/FlexCore.Logging.Interfaces.Tests/ILoggingProviderExtensionsTests.cs
using Xunit;
using FlexCore.Logging.Interfaces;

namespace FlexCore.Logging.Interfaces.Tests;

/// <summary>
/// Test per gli extension methods di <see cref="ILoggingProvider"/>
/// </summary>
public class ILoggingProviderExtensionsTests
{
    private class TestProvider : ILoggingProvider
    {
        public string LastLevel { get; private set; } = string.Empty;
        public string LastMessage { get; private set; } = string.Empty;

        public void Debug(string message)
        {
            LastLevel = "DEBUG";
            LastMessage = message;
        }

        public void Info(string message)
        {
            LastLevel = "INFO";
            LastMessage = message;
        }

        public void Warn(string message)
        {
            LastLevel = "WARN";
            LastMessage = message;
        }

        public void Error(string message)
        {
            LastLevel = "ERROR";
            LastMessage = message;
        }

        public void Fatal(string message)
        {
            LastLevel = "FATAL";
            LastMessage = message;
        }
    }

    /// <summary>
    /// Verifica che Debug invochi il metodo corretto
    /// </summary>
    [Fact]
    public void Debug_ShouldCallDebugMethod()
    {
        var provider = new TestProvider();
        provider.Debug("test message");
        Assert.Equal("DEBUG", provider.LastLevel);
        Assert.Equal("test message", provider.LastMessage);
    }

    /// <summary>
    /// Verifica che Info invochi il metodo corretto
    /// </summary>
    [Fact]
    public void Info_ShouldCallInfoMethod()
    {
        var provider = new TestProvider();
        provider.Info("test message");
        Assert.Equal("INFO", provider.LastLevel);
        Assert.Equal("test message", provider.LastMessage);
    }
}