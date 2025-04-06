using Xunit;
using FlexCore.Caching.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;

namespace FlexCore.Caching.Core.Tests
{
    public class CacheServiceFullCoverageTests
    {
        [Theory]
        [InlineData(null)]    // Test esplicito per null
        [InlineData("")]      // Stringa vuota
        [InlineData("   ")]   // Spazi bianchi
        [InlineData("key!")]  // Carattere speciale
        public async Task GetAsync_InvalidKey_ShouldThrowArgumentException(string? invalidKey)
        {
            // Arrange
            var mockProvider = new Mock<ICacheProvider>();
            var mockLogger = new Mock<ILogger<ICacheService>>();
            var cacheService = new ConcreteCacheService(mockProvider.Object, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                cacheService.GetAsync<string>(invalidKey!)); // ! solo per bypassare il warning nel test
        }

        [Fact]
        public async Task ClearAllAsync_ShouldCallProvider()
        {
            // Arrange
            var mockProvider = new Mock<ICacheProvider>();
            var mockLogger = new Mock<ILogger<ICacheService>>();
            var cacheService = new ConcreteCacheService(mockProvider.Object, mockLogger.Object);

            // Act
            await cacheService.ClearAllAsync();

            // Assert
            mockProvider.Verify(p => p.ClearAllAsync(), Times.Once);
        }

        private class ConcreteCacheService : ICacheService
        {
            private readonly ICacheProvider _provider;

            public ConcreteCacheService(ICacheProvider provider, ILogger<ICacheService> logger)
            {
                _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            }

            public Task<T?> GetAsync<T>(string key)
            {
                if (key is null) throw new ArgumentNullException(nameof(key));
                return _provider.GetAsync<T>(key);
            }

            public async Task<bool> SetAsync<T>(string key, T value, TimeSpan expiration)
            {
                if (key is null) throw new ArgumentNullException(nameof(key));
                return await _provider.SetAsync(key, value, expiration);
            }

            public async Task<bool> RemoveAsync(string key)
            {
                if (key is null) throw new ArgumentNullException(nameof(key));
                return await _provider.RemoveAsync(key);
            }

            public async Task<bool> ExistsAsync(string key)
            {
                if (key is null) throw new ArgumentNullException(nameof(key));
                return await _provider.ExistsAsync(key);
            }

            public async Task ClearAllAsync() => await _provider.ClearAllAsync();
        }
    }
}