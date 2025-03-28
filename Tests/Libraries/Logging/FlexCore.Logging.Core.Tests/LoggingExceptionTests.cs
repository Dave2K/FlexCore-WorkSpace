// File: Tests/FlexCore.Logging.Core.Tests/LoggingExceptionTests.cs
using Xunit;
using System;
using FleFlexCore.Logging.Core.Exceptions;

namespace FlexCore.Logging.Core.Tests;

public class LoggingExceptionTests
{
    [Fact]
    public void Constructor_WithMessageAndInnerException_ShouldSetBoth()
    {
        var innerEx = new Exception("Inner error");
        var ex = new LoggingException("Test message", innerEx);
        Assert.Equal("Test message", ex.Message);
        Assert.Equal(innerEx, ex.InnerException);
    }
}