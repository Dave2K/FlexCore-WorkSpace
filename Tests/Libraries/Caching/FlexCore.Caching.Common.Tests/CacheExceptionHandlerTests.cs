using Xunit;
using FlexCore.Caching.Common.Exceptions;
using System;

namespace FlexCore.Caching.Common.Tests;

public class CacheExceptionHandlerTests
{
    [Fact]
    public void HandleException_ThrowsCacheException_ForGenericException()
    {
        var exception = new Exception("Generic error");
        Assert.Throws<CacheException>(() => CacheExceptionHandler.HandleException(exception, "operation"));
    }

    [Fact]
    public void HandleException_ThrowsCacheException_ForRedisCacheException()
    {
        var exception = new RedisCacheException("Redis error");
        var ex = Assert.Throws<CacheException>(() => CacheExceptionHandler.HandleException(exception, "operation"));
        Assert.Contains("Redis", ex.Message);
    }

    [Fact]
    public void HandleException_ThrowsCacheException_ForMemoryCacheException()
    {
        var exception = new MemoryCacheException("Memory error");
        var ex = Assert.Throws<CacheException>(() => CacheExceptionHandler.HandleException(exception, "operation"));
        Assert.Contains("memoria", ex.Message);
    }
}