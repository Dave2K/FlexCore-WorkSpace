using Xunit;
using FlexCore.Caching.Core.Interfaces;
using FlexCore.Caching.Common.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FlexCore.Caching.Core.Tests
{
    /// <summary>
    /// Test completi per verificare tutti gli scenari del servizio di cache
    /// </summary>
    public class CacheServiceFullCoverageTests
    {
        [Theory]
        [InlineData(null!)] // Test esplicito per null con null-forgiving operator
        [InlineData("")]
        [InlineData("invalid key!")]
        public async Task GetAsync_InvalidKey_ShouldThrowArgumentException(string invalidKey)
        {
            var mockProvider = new Mock<ICacheProvider>();
            var mockLogger = new Mock<ILogger<ICacheService>>();
            var cacheService = new ConcreteCacheService(mockProvider.Object, mockLogger.Object);

            await Assert.ThrowsAsync<ArgumentException>(
                () => cacheService.GetAsync<string>(invalidKey)
            );
        }

        // Implementazione helper per il test
        private class ConcreteCacheService : ICacheService
        {
            private readonly ICacheProvider _provider;
            private readonly ILogger<ICacheService> _logger;

            public ConcreteCacheService(ICacheProvider provider, ILogger<ICacheService> logger)
            {
                _provider = provider;
                _logger = logger;
            }

            public T? Get<T>(string key) => throw new System.NotImplementedException();
            public void Set<T>(string key, T value, TimeSpan expiration) => throw new System.NotImplementedException();
            public bool Remove(string key) => throw new System.NotImplementedException();
            public bool Exists(string key) => throw new System.NotImplementedException();

            public Task<T?> GetAsync<T>(string key) => _provider.GetAsync<T>(key);
            public Task SetAsync<T>(string key, T value, TimeSpan expiration) => _provider.SetAsync(key, value, expiration);
            public Task<bool> RemoveAsync(string key) => _provider.RemoveAsync(key);
            public Task<bool> ExistsAsync(string key) => _provider.ExistsAsync(key);
        }
    }
}