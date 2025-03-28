// File: Tests/FlexCore.Logging.Interfaces.Tests/ILoggerTests.cs
using Xunit;
using FlexCore.Logging.Core;
using FlexCore.Logging.Interfaces;

namespace FlexCore.Logging.Core.Tests;

/// <summary>
/// Test per l'interfaccia <see cref="ILogger"/>
/// </summary>
public class ILoggerTests
{
    private class TestLogger : ILogger
    {
        public string LastMessage { get; private set; } = string.Empty;
        public string LastLevel { get; private set; } = string.Empty;

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
    /// Verifica che Debug registri correttamente il messaggio
    /// </summary>
    [Fact]
    public void Debug_ShouldLogMessage()
    {
        var logger = new TestLogger();
        logger.Debug("Test debug");
        Assert.Equal("DEBUG", logger.LastLevel);
        Assert.Equal("Test debug", logger.LastMessage);
    }

    /// <summary>
    /// Verifica che Info registri correttamente il messaggio
    /// </summary>
    [Fact]
    public void Info_ShouldLogMessage()
    {
        var logger = new TestLogger();
        logger.Info("Test info");
        Assert.Equal("INFO", logger.LastLevel);
        Assert.Equal("Test info", logger.LastMessage);
    }

    /// <summary>
    /// Verifica che Warn registri correttamente il messaggio
    /// </summary>
    [Fact]
    public void Warn_ShouldLogMessage()
    {
        var logger = new TestLogger();
        logger.Warn("Test warn");
        Assert.Equal("WARN", logger.LastLevel);
        Assert.Equal("Test warn", logger.LastMessage);
    }

    /// <summary>
    /// Verifica che Error registri correttamente il messaggio
    /// </summary>
    [Fact]
    public void Error_ShouldLogMessage()
    {
        var logger = new TestLogger();
        logger.Error("Test error");
        Assert.Equal("ERROR", logger.LastLevel);
        Assert.Equal("Test error", logger.LastMessage);
    }

    /// <summary>
    /// Verifica che Fatal registri correttamente il messaggio
    /// </summary>
    [Fact]
    public void Fatal_ShouldLogMessage()
    {
        var logger = new TestLogger();
        logger.Fatal("Test fatal");
        Assert.Equal("FATAL", logger.LastLevel);
        Assert.Equal("Test fatal", logger.LastMessage);
    }
}