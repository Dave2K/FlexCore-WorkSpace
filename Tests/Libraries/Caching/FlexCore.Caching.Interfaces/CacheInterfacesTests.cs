using Xunit;
using FlexCore.Caching.Interfaces;
using System;
using System.Collections.Generic;

public class CacheInterfacesTests
{
    [Fact]
    public void ICacheProvider_Set_AddsValueToCache()
    {
        var cacheProvider = new TestCacheProvider();
        cacheProvider.Set("key", "value", TimeSpan.FromMinutes(1));
        var result = cacheProvider.Get<string>("key");
        Assert.NotNull(result);
        Assert.Equal("value", result);
    }

    private class TestCacheProvider : ICacheProvider
    {
        private readonly Dictionary<string, object> _cache = new();

        public T Get<T>(string key) => _cache.ContainsKey(key) ? (T)_cache[key] : default!;

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            _cache[key] = value ?? throw new ArgumentNullException(nameof(value));
        }

        public void Remove(string key) => _cache.Remove(key);

        public bool Exists(string key) => _cache.ContainsKey(key);
    }
}