using Microsoft.Extensions.Caching.Memory;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging; // Aggiungi questo using

namespace FlexCore.Caching.Memory.Tests
{
    public class MemoryCacheProviderTests
    {
        private readonly MemoryCacheProvider _provider;
        private readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());
        private readonly Mock<ILogger<MemoryCacheProvider>> _loggerMock = new Mock<ILogger<MemoryCacheProvider>>(); // Aggiungi il mock del logger

        public MemoryCacheProviderTests()
        {
            // Passa il logger mockato al costruttore
            _provider = new MemoryCacheProvider(_memoryCache, _loggerMock.Object);
        }

        [Fact]
        public async Task SetAndGet_ValidKey_ReturnsValue() // Rendi il metodo async
        {
            // Arrange
            const string key = "test_key";
            const string expectedValue = "test_value";

            // Act
            await _provider.SetAsync(key, expectedValue, TimeSpan.FromMinutes(5)); // Usa SetAsync
            var result = await _provider.GetAsync<string>(key); // Usa GetAsync

            // Assert
            Assert.Equal(expectedValue, result);
        }
    }
}