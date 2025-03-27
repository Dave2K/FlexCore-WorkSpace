namespace FlexCore.Caching.Factory.Tests
{
    using Xunit;
    using FlexCore.Caching.Factory;
    using FlexCore.Caching.Interfaces;
    using System;
    using System.Collections.Generic;

    public class CacheProviderFactoryTests
    {
        [Fact]
        public void RegisterProvider_ThrowsArgumentException_WhenNameIsNullOrEmpty()
        {
            var factory = new CacheProviderFactory();
            Assert.Throws<ArgumentException>(() => factory.RegisterProvider("", () => null!));
            Assert.Throws<ArgumentException>(() => factory.RegisterProvider(null!, () => new TestCacheProvider()));
        }

        [Fact]
        public void RegisterProvider_ThrowsArgumentNullException_WhenProviderFactoryIsNull()
        {
            var factory = new CacheProviderFactory();
            Assert.Throws<ArgumentNullException>(() => factory.RegisterProvider("test", null!));
        }

        [Fact]
        public void CreateProvider_ReturnsSameInstance_WhenProviderIsRegistered()
        {
            var factory = new CacheProviderFactory();
            var expectedProvider = new TestCacheProvider();
            factory.RegisterProvider("test", () => expectedProvider);

            var provider1 = factory.CreateProvider("test");
            var provider2 = factory.CreateProvider("test");

            Assert.Same(expectedProvider, provider1);
            Assert.Same(expectedProvider, provider2);
        }

        [Fact]
        public void CreateProvider_ThrowsArgumentNullException_WhenProviderNameIsNull()
        {
            var factory = new CacheProviderFactory();
            Assert.Throws<ArgumentNullException>(() => factory.CreateProvider(null!));
        }

        [Fact]
        public void CreateProvider_ThrowsNotSupportedException_WhenProviderIsNotRegistered()
        {
            var factory = new CacheProviderFactory();
            Assert.Throws<NotSupportedException>(() => factory.CreateProvider(""));
            Assert.Throws<NotSupportedException>(() => factory.CreateProvider("nonexistent"));
        }

        [Fact]
        public void Remove_RemovesKeyFromCache()
        {
            var cache = new TestCacheProvider();
            cache.Set("key", "value", TimeSpan.FromMinutes(1));
            cache.Remove("key");
            var result = cache.Get<string>("key");
            Assert.Null(result);
        }

        private class TestCacheProvider : ICacheProvider
        {
            private readonly Dictionary<string, object> _cache = new();

            public T Get<T>(string key) => string.IsNullOrEmpty(key) ? throw new ArgumentNullException(nameof(key)) : _cache.TryGetValue(key, out var value) ? (T)value! : default!;

            public void Set<T>(string key, T value, TimeSpan expiration)
            {
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentNullException(nameof(key), "La chiave non può essere null o vuota.");
                if (value is null)
                    throw new ArgumentNullException(nameof(value), "Il valore non può essere null.");
                _cache[key] = value;
            }

            public void Remove(string key)
            {
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentNullException(nameof(key), "La chiave non può essere null o vuota.");
                _cache.Remove(key);
            }

            public bool Exists(string key) => !string.IsNullOrEmpty(key) && _cache.TryGetValue(key, out _);
        }
    }
}