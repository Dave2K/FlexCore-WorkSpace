using Xunit;
using FlexCore.Caching.Common.Exceptions;

namespace FlexCore.Caching.Common.Tests;

public class CacheExceptionTests
{
    [Fact]
    public void CacheException_InitializesWithMessage()
    {
        var exception = new CacheException("Test message");
        Assert.Equal("Test message", exception.Message);
    }

    [Fact]
    public void CacheException_InitializesWithMessageAndInnerException()
    {
        var innerException = new Exception("Inner exception");
        var exception = new CacheException("Test message", innerException);
        Assert.Equal("Test message", exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }

    [Fact]
    public void MemoryCacheException_InitializesWithMessage()
    {
        var exception = new MemoryCacheException("Test message");
        Assert.Equal("Test message", exception.Message);
    }

    [Fact]
    public void MemoryCacheException_InitializesWithMessageAndInnerException()
    {
        var innerException = new Exception("Inner exception");
        var exception = new MemoryCacheException("Test message", innerException);
        Assert.Equal("Test message", exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }

    [Fact]
    public void RedisCacheException_InitializesWithMessage()
    {
        var exception = new RedisCacheException("Test message");
        Assert.Equal("Test message", exception.Message);
    }

    [Fact]
    public void RedisCacheException_InitializesWithMessageAndInnerException()
    {
        var innerException = new Exception("Inner exception");
        var exception = new RedisCacheException("Test message", innerException);
        Assert.Equal("Test message", exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }
}