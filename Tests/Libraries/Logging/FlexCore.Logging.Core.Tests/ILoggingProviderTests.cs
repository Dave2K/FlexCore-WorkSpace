// File: Tests/FlexCore.Logging.Interfaces.Tests/ILoggingProviderTests.cs
using Xunit;
using FlexCore.Logging.Interfaces;

namespace FlexCore.Logging.Core.Tests;

/// <summary>
/// Test per l'interfaccia <see cref="ILoggingProvider"/>
/// </summary>
public class ILoggingProviderTests
{
    private class TestProvider : ILoggingProvider
    {
        public string LastMessage { get; private set; } = string.Empty;

        public void Debug(string message) => LastMessage = $"DEBUG: {message}";
        public void Info(string message) => LastMessage = $"INFO: {message}";
        public void Warn(string message) => LastMessage = $"WARN: {message}";
        public void Error(string message) => LastMessage = $"ERROR: {message}";
        public void Fatal(string message) => LastMessage = $"FATAL: {message}";
    }

    /// <summary>
    /// Verifica che ILoggingProvider implementi correttamente ILogger
    /// </summary>
    [Fact]
    public void ShouldImplementILoggerInterface()
    {
        ILoggingProvider provider = new TestProvider();
        provider.Debug("Test");
        Assert.StartsWith("DEBUG:", ((TestProvider)provider).LastMessage);
    }
}