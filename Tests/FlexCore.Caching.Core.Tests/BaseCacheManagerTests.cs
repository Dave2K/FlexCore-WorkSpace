namespace FlexCore.Caching.Core.Tests;

using Xunit;
using FlexCore.Caching.Core;
using FlexCore.Caching.Interfaces;
using System;
using System.Collections.Generic;

public class BaseCacheManagerTests
{
    [Fact]
    public void Get_ReturnsDefault_WhenKeyNotExists()
    {
        var cacheManager = new TestCacheManager();
        var result = cacheManager.Get<string>("nonexistent_key");
        Assert.Null(result);
    }

    [Fact]
    public void Set_AddsValueToCache()
    {
        var cacheManager = new TestCacheManager();
        cacheManager.Set("key", "value", TimeSpan.FromMinutes(1));
        var result = cacheManager.Get<string>("key");
        Assert.Equal("value", result);
    }

    [Fact]
    public void Remove_DeletesValueFromCache()
    {
        var cacheManager = new TestCacheManager();
        cacheManager.Set("key", "value", TimeSpan.FromMinutes(1));
        cacheManager.Remove("key");
        var result = cacheManager.Get<string>("key");
        Assert.Null(result);
    }

    [Fact]
    public void Exists_ReturnsTrue_WhenKeyExists()
    {
        var cacheManager = new TestCacheManager();
        cacheManager.Set("key", "value", TimeSpan.FromMinutes(1));
        Assert.True(cacheManager.Exists("key"));
    }

    private class TestCacheManager : BaseCacheManager
    {
        private readonly Dictionary<string, object> _cache = new();

        public override T Get<T>(string key) => _cache.ContainsKey(key) ? (T)_cache[key] : default!;

        public override void Set<T>(string key, T value, TimeSpan expiration)
        {
            if (value is null) throw new ArgumentNullException(nameof(value), "Il valore non può essere null.");
            _cache[key] = value;
        }

        public override void Remove(string key) => _cache.Remove(key);

        public override bool Exists(string key) => _cache.ContainsKey(key);
    }
}
