using Xunit;
using FlexCore.Caching.Redis;
using FlexCore.Caching.Interfaces;
using StackExchange.Redis;
using System;
using System.Text.Json;

public class RedisCacheProviderTests
{
    [Fact(Skip = "Redis non installato, test disabilitato temporaneamente")]
    public void Get_ReturnsDefault_WhenKeyNotExists()
    {
        var connection = ConnectionMultiplexer.Connect("localhost");
        var provider = new RedisCacheProvider(connection);
        var result = provider.Get<string>("nonexistent_key");
        Assert.Null(result);
    }

    [Fact(Skip = "Redis non installato, test disabilitato temporaneamente")]
    public void Set_AddsValueToCache()
    {
        var connection = ConnectionMultiplexer.Connect("localhost");
        var provider = new RedisCacheProvider(connection);
        provider.Set("key", "value", TimeSpan.FromMinutes(1));
        var result = provider.Get<string>("key");
        Assert.Equal("value", result);
    }

    [Fact(Skip = "Redis non installato, test disabilitato temporaneamente")]
    public void Remove_DeletesValueFromCache()
    {
        var connection = ConnectionMultiplexer.Connect("localhost");
        var provider = new RedisCacheProvider(connection);
        provider.Set("key", "value", TimeSpan.FromMinutes(1));
        provider.Remove("key");
        var result = provider.Get<string>("key");
        Assert.Null(result);
    }

    [Fact(Skip = "Redis non installato, test disabilitato temporaneamente")]
    public void Exists_ReturnsTrue_WhenKeyExists()
    {
        var connection = ConnectionMultiplexer.Connect("localhost");
        var provider = new RedisCacheProvider(connection);
        provider.Set("key", "value", TimeSpan.FromMinutes(1));
        Assert.True(provider.Exists("key"));
    }
}
