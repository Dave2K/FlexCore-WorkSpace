using FlexCore.Caching.Core.Interfaces;
using Xunit;
using System;
using System.Threading.Tasks;
using FlexCore.Caching.Memory;
using FlexCore.Caching.Redis;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;

namespace FlexCore.Caching.Tests.Integration
{
    public class CrossProviderTests : IDisposable
    {
        private readonly ICacheProvider _provider;
        private readonly IDisposable _disposableProvider;

        public CrossProviderTests()
        {
            _provider = CreateProvider("Memory"); // Cambia in "Redis" per testare Redis
            _disposableProvider = _provider as IDisposable;
        }

        [Theory]
        [InlineData("test_key")]
        public async Task SetAndGet_ValidKey_ReturnsValue(string key)
        {
            var expectedValue = 42;
            await _provider.SetAsync(key, expectedValue, TimeSpan.FromMinutes(1));
            var actualValue = await _provider.GetAsync<int>(key);
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("invalid_key")]
        public async Task Get_InvalidKey_ReturnsDefault(string key)
        {
            var value = await _provider.GetAsync<string>(key);
            Assert.Null(value);
        }

        [Theory]
        [InlineData("expiring_key")]
        public async Task SetWithExpiry_KeyExpires_ReturnsNullAfterDelay(string key)
        {
            var expiry = TimeSpan.FromSeconds(2);
            await _provider.SetAsync(key, "value", expiry);
            await Task.Delay(expiry.Add(TimeSpan.FromSeconds(1)));
            var value = await _provider.GetAsync<string>(key);
            Assert.Null(value);
        }

        public void Dispose()
        {
            _disposableProvider?.Dispose();
        }

        private ICacheProvider CreateProvider(string providerType)
        {
            return providerType switch
            {
                "Redis" => new RedisCacheProvider(
                    "localhost:6379",
                    Mock.Of<ILogger<RedisCacheProvider>>(),
                    ConnectionMultiplexer.Connect("localhost:6379")
                ),
                "Memory" => new MemoryCacheProvider(
                    new MemoryCache(new MemoryCacheOptions()),
                    Mock.Of<ILogger<MemoryCacheProvider>>()
                ),
                _ => throw new NotSupportedException()
            };
        }
    }
}