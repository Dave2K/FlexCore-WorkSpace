namespace FlexCore.Caching.Common.Tests;

using FlexCore.Caching.Common.Exceptions;
using System;
using Xunit;

public class CacheExceptionTests
{
    [Fact]
    public void CacheException_Constructor_WithMessage()
    {
        var message = "Test exception";
        var exception = new CacheException(message);
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void CacheException_Constructor_WithMessageAndInnerException()
    {
        var message = "Test exception";
        var innerException = new Exception("Inner exception");
        var exception = new CacheException(message, innerException);
        Assert.Equal(message, exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }
}