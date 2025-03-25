// File: Tests/FlexCore.Logging.SerilogTests/SerilogLoggerTests.cs
using Xunit;
using FlexCore.Logging.Serilog;
using FlexCore.Logging.Interfaces;
using Serilog;
using Serilog.Sinks.TestCorrelator;
using System;
using System.Linq;
using Serilog.Events; // Aggiunto questo using

namespace FlexCore.Logging.SerilogTests;

/// <summary>
/// Test per la classe <see cref="SerilogLogger"/>
/// </summary>
public class SerilogLoggerTests : IDisposable
{
    private readonly SerilogLogger _logger;

    public SerilogLoggerTests()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.TestCorrelator()
            .CreateLogger();

        _logger = new SerilogLogger();
    }

    public void Dispose()
    {
        Log.CloseAndFlush();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void Debug_ShouldLogMessage()
    {
        using (TestCorrelator.CreateContext())
        {
            _logger.Debug("Test debug message");
            Assert.Contains(TestCorrelator.GetLogEventsFromCurrentContext(),
                le => le.MessageTemplate.Text == "Test debug message" &&
                      le.Level == LogEventLevel.Debug); // Modificato qui
        }
    }

    [Fact]
    public void Info_ShouldLogMessage()
    {
        using (TestCorrelator.CreateContext())
        {
            _logger.Info("Test info message");
            Assert.Contains(TestCorrelator.GetLogEventsFromCurrentContext(),
                le => le.MessageTemplate.Text == "Test info message" &&
                      le.Level == LogEventLevel.Information); // Modificato qui
        }
    }

    [Fact]
    public void Warn_ShouldLogMessage()
    {
        using (TestCorrelator.CreateContext())
        {
            _logger.Warn("Test warn message");
            Assert.Contains(TestCorrelator.GetLogEventsFromCurrentContext(),
                le => le.MessageTemplate.Text == "Test warn message" &&
                      le.Level == LogEventLevel.Warning); // Modificato qui
        }
    }

    [Fact]
    public void Error_ShouldLogMessage()
    {
        using (TestCorrelator.CreateContext())
        {
            _logger.Error("Test error message");
            Assert.Contains(TestCorrelator.GetLogEventsFromCurrentContext(),
                le => le.MessageTemplate.Text == "Test error message" &&
                      le.Level == LogEventLevel.Error); // Modificato qui
        }
    }

    [Fact]
    public void Fatal_ShouldLogMessage()
    {
        using (TestCorrelator.CreateContext())
        {
            _logger.Fatal("Test fatal message");
            Assert.Contains(TestCorrelator.GetLogEventsFromCurrentContext(),
                le => le.MessageTemplate.Text == "Test fatal message" &&
                      le.Level == LogEventLevel.Fatal); // Modificato qui
        }
    }

    [Fact]
    public void ShouldImplementILoggingProvider()
    {
        Assert.IsType<SerilogLogger>(_logger);
    }
}