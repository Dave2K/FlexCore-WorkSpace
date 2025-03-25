// File: Tests/FlexCore.Logging.Log4NetTests/Log4NetLoggerTests.cs
using Xunit;
using FlexCore.Logging.Log4Net;
using FlexCore.Logging.Interfaces;
using System;
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Core;
using log4net.Appender;

namespace FlexCore.Logging.Log4NetTests;

/// <summary>
/// Test per la classe <see cref="Log4NetLogger"/>
/// </summary>
public class Log4NetLoggerTests : IDisposable
{
    private readonly MemoryAppender _memoryAppender;
    private readonly ILoggingProvider _logger;

    public Log4NetLoggerTests()
    {
        var hierarchy = (Hierarchy)LogManager.GetRepository();
        _memoryAppender = new MemoryAppender();
        hierarchy.Root.AddAppender(_memoryAppender);
        hierarchy.Root.Level = Level.All;
        hierarchy.Configured = true;

        _logger = new Log4NetLogger();
    }

    public void Dispose()
    {
        _memoryAppender.Clear();
        LogManager.Shutdown();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void Debug_ShouldLogMessage()
    {
        _logger.Debug("Test message");
        Assert.Single(_memoryAppender.GetEvents());
        Assert.Equal("Test message", _memoryAppender.GetEvents()[0].RenderedMessage);
        Assert.Equal(Level.Debug, _memoryAppender.GetEvents()[0].Level);
    }
}