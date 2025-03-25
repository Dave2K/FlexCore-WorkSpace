using Xunit;
using FlexCore.Caching.Memory;
using FlexCore.Caching.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;

public class MemoryCacheProviderTests
{
    [Fact]
    public void Get_ReturnsDefault_WhenKeyNotExists()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var provider = new MemoryCacheProvider(memoryCache);
        var result = provider.Get<string>("nonexistent_key");
        Assert.Null(result);
    }

    [Fact]
    public void Set_AddsValueToCache()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var provider = new MemoryCacheProvider(memoryCache);
        provider.Set("key", "value", TimeSpan.FromMinutes(1));
        var result = provider.Get<string>("key");
        Assert.Equal("value", result);
    }

    [Fact]
    public void Remove_DeletesValueFromCache()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var provider = new MemoryCacheProvider(memoryCache);
        provider.Set("key", "value", TimeSpan.FromMinutes(1));
        provider.Remove("key");
        var result = provider.Get<string>("key");
        Assert.Null(result);
    }

    [Fact]
    public void Exists_ReturnsTrue_WhenKeyExists()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var provider = new MemoryCacheProvider(memoryCache);
        provider.Set("key", "value", TimeSpan.FromMinutes(1));
        Assert.True(provider.Exists("key"));
    }

    [Fact]
    public void ClearAll_RemovesAllValuesFromCache()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var provider = new MemoryCacheProvider(memoryCache);
        provider.Set("key1", "value1", TimeSpan.FromMinutes(1));
        provider.Set("key2", "value2", TimeSpan.FromMinutes(1));
        provider.ClearAll();
        Assert.False(provider.Exists("key1"));
        Assert.False(provider.Exists("key2"));
    }
}