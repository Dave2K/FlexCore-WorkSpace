namespace FlexCore.Core.Utilities.Tests;

using FlexCore.Core.Utilities;
using System;
using Xunit;

public class ExceptionHandlerTests
{
    [Fact]
    public void HandleException_ThrowsCustomException_WhenExceptionOccurs()
    {
        var ex = new Exception("Test exception");
        const string operation = "Test operation";

        Func<Exception, string, Exception> customExceptionFactory = (e, op) =>
            new InvalidOperationException($"Errore durante {op}: {e.Message}");

        Assert.Throws<InvalidOperationException>(() =>
            ExceptionHandler.HandleException(ex, operation, customExceptionFactory));
    }

    [Fact]
    public void HandleException_ThrowsCorrectExceptionMessage_WhenExceptionOccurs()
    {
        var ex = new Exception("Test exception");
        const string operation = "Test operation";

        Func<Exception, string, Exception> customExceptionFactory = (e, op) =>
            new InvalidOperationException($"Errore durante {op}: {e.Message}");

        var exception = Assert.Throws<InvalidOperationException>(() =>
            ExceptionHandler.HandleException(ex, operation, customExceptionFactory));

        Assert.Equal("Errore durante Test operation: Test exception", exception.Message);
    }
}