using FlexCore.Caching.Core.Interfaces;
using FlexCore.Caching.Common.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FlexCore.Caching.Core.Tests
{
    /// <summary>
    /// Test per garantire la copertura completa della classe <see cref="ICacheService"/>
    /// </summary>
    public class CacheServiceFullCoverageTests
    {
        private readonly Mock<ICacheProvider> _mockProvider;
        private readonly Mock<ILogger<ICacheService>> _mockLogger;
        private readonly ICacheService _cacheService;

        public CacheServiceFullCoverageTests()
        {
            _mockProvider = new Mock<ICacheProvider>();
            _mockLogger = new Mock<ILogger<ICacheService>>();
            _cacheService = new ConcreteCacheService(_mockProvider.Object, _mockLogger.Object); // Sostituire con l'implementazione reale
        }

        /// <summary>
        /// Verifica il recupero di un elemento esistente
        /// </summary>
        [Fact]
        public async Task GetAsync_ExistingKey_ShouldReturnValue()
        {
            // Arrange
            const string key = "valid_key";
            var expectedValue = "test_value";
            _mockProvider.Setup(p => p.ExistsAsync(key)).ReturnsAsync(true);
            _mockProvider.Setup(p => p.GetAsync<string>(key)).ReturnsAsync(expectedValue);

            // Act
            var result = await _cacheService.GetAsync<string>(key);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        /// <summary>
        /// Verifica il comportamento con chiave non esistente
        /// </summary>
        [Fact]
        public async Task GetAsync_NonExistingKey_ShouldReturnDefault()
        {
            // Arrange
            const string key = "ghost_key";
            _mockProvider.Setup(p => p.ExistsAsync(key)).ReturnsAsync(false);

            // Act
            var result = await _cacheService.GetAsync<string>(key);

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Verifica il lancio di eccezione per chiave non valida
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid key!")]
        public async Task GetAsync_InvalidKey_ShouldThrowArgumentException(string invalidKey)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _cacheService.GetAsync<string>(invalidKey));
        }

        /// <summary>
        /// Verifica il salvataggio corretto di un elemento
        /// </summary>
        [Fact]
        public async Task SetAsync_ValidParameters_ShouldCallProvider()
        {
            // Arrange
            const string key = "test_key";
            var value = new { Id = 1, Name = "Test" };

            // Act
            await _cacheService.SetAsync(key, value, TimeSpan.FromMinutes(5));

            // Assert
            _mockProvider.Verify(p =>
                p.SetAsync(key, value, TimeSpan.FromMinutes(5)),
                Times.Once
            );
        }

        // Implementazione fittizia per test
        private class ConcreteCacheService : ICacheService
        {
            private readonly ICacheProvider _provider;
            private readonly ILogger<ICacheService> _logger;

            public ConcreteCacheService(ICacheProvider provider, ILogger<ICacheService> logger)
            {
                _provider = provider;
                _logger = logger;
            }

            public T? Get<T>(string key) => throw new NotImplementedException();
            public Task<T?> GetAsync<T>(string key) => _provider.GetAsync<T>(key);
            public void Set<T>(string key, T value, TimeSpan expiration) => throw new NotImplementedException();
            public Task SetAsync<T>(string key, T value, TimeSpan expiration) => _provider.SetAsync(key, value, expiration);
            public bool Remove(string key) => throw new NotImplementedException();
            public Task<bool> RemoveAsync(string key) => _provider.RemoveAsync(key);
            public bool Exists(string key) => throw new NotImplementedException();
            public Task<bool> ExistsAsync(string key) => _provider.ExistsAsync(key);
        }
    }
}