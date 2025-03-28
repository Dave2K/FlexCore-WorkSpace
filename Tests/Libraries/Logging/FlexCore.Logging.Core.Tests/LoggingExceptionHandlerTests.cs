using Xunit;
using System;
using FleFlexCore.Logging.Core.Base;
using FleFlexCore.Logging.Core.Exceptions;

namespace FlexCore.Logging.Core.Tests;

public class LoggingExceptionHandlerTests
{
    [Fact]
    public void HandleException_ShouldWrapGenericException()
    {
        var originalEx = new Exception("Test error");
        const string operation = "TestOperation";

        var resultEx = Assert.Throws<LoggingException>(
            () => LoggingExceptionHandler.HandleException(originalEx, operation));

        Assert.Equal(originalEx, resultEx.InnerException);
        Assert.Contains(operation, resultEx.Message);
    }

    [Fact]
    public void HandleException_ShouldAddLog4NetPrefix()
    {
        var originalEx = new Log4NetException("Config error");
        const string operation = "Log4NetConfig";

        var resultEx = Assert.Throws<LoggingException>(
            () => LoggingExceptionHandler.HandleException(originalEx, operation));

        Assert.Contains("Log4Net", resultEx.Message);
        Assert.Equal(originalEx, resultEx.InnerException);
    }

    [Fact]
    public void HandleException_ShouldHandleNullOperation()
    {
        var originalEx = new Exception("Test error");

        var resultEx = Assert.Throws<LoggingException>(
            () => LoggingExceptionHandler.HandleException(originalEx, null!));

        Assert.Contains("logging", resultEx.Message);
    }
}