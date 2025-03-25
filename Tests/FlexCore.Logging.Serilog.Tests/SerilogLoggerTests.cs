using Xunit;
using FlexCore.Logging.Serilog;
using global::Serilog;
using Serilog.Sinks.TestCorrelator;
using System;
using System.Linq;
using Serilog.Events;
using FlexCore.Logging.Interfaces;

namespace FlexCore.Logging.SerilogTests;

public class SerilogLoggerTests : IDisposable
{
    private readonly global::Serilog.ILogger _serilogLogger;
    private readonly ILoggingProvider _logger;

    public SerilogLoggerTests()
    {
        _serilogLogger = new LoggerConfiguration()
            .MinimumLevel.Is(LevelAlias.Minimum)
            .WriteTo.TestCorrelator()
            .CreateLogger();

        Log.Logger = _serilogLogger;
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

            var events = TestCorrelator.GetLogEventsFromCurrentContext().ToList();
            Assert.NotEmpty(events);
            Assert.Contains(events, e =>
                e.MessageTemplate.Text == "Test debug message" &&
                e.Level == LogEventLevel.Debug);
        }
    }

    [Fact]
    public void Info_ShouldLogMessage()
    {
        using (TestCorrelator.CreateContext())
        {
            _logger.Info("Test info message");

            var events = TestCorrelator.GetLogEventsFromCurrentContext().ToList();
            Assert.NotEmpty(events);
            Assert.Contains(events, e =>
                e.MessageTemplate.Text == "Test info message" &&
                e.Level == LogEventLevel.Information);
        }
    }

    [Fact]
    public void Warn_ShouldLogMessage()
    {
        using (TestCorrelator.CreateContext())
        {
            _logger.Warn("Test warn message");

            var events = TestCorrelator.GetLogEventsFromCurrentContext().ToList();
            Assert.NotEmpty(events);
            Assert.Contains(events, e =>
                e.MessageTemplate.Text == "Test warn message" &&
                e.Level == LogEventLevel.Warning);
        }
    }

    [Fact]
    public void Error_ShouldLogMessage()
    {
        using (TestCorrelator.CreateContext())
        {
            _logger.Error("Test error message");

            var events = TestCorrelator.GetLogEventsFromCurrentContext().ToList();
            Assert.NotEmpty(events);
            Assert.Contains(events, e =>
                e.MessageTemplate.Text == "Test error message" &&
                e.Level == LogEventLevel.Error);
        }
    }

    [Fact]
    public void Fatal_ShouldLogMessage()
    {
        using (TestCorrelator.CreateContext())
        {
            _logger.Fatal("Test fatal message");

            var events = TestCorrelator.GetLogEventsFromCurrentContext().ToList();
            Assert.NotEmpty(events);
            Assert.Contains(events, e =>
                e.MessageTemplate.Text == "Test fatal message" &&
                e.Level == LogEventLevel.Fatal);
        }
    }

    [Fact]
    public void ShouldImplementILoggingProvider()
    {
        Assert.IsAssignableFrom<ILoggingProvider>(_logger);
    }
}