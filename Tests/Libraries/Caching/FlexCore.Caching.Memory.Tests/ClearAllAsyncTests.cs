using Xunit;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Moq;

namespace FlexCore.Caching.Memory.Tests
{
    /// <summary>
    /// Test per il metodo ClearAllAsync di MemoryCacheProvider
    /// </summary>
    public class ClearAllAsyncTests
    {
        /// <summary>
        /// Verifica che lo svuotamento della cache rimuova tutte le chiavi
        /// </summary>
        [Fact]
        public async Task ClearAllAsync_RemovesAllKeys()
        {
            // Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());
            var logger = Mock.Of<ILogger<MemoryCacheProvider>>();
            var provider = new MemoryCacheProvider(cache, logger);

            // Aggiungi dati alla cache
            await provider.SetAsync("key1", "value1", TimeSpan.FromMinutes(1));
            await provider.SetAsync("key2", 100, TimeSpan.FromMinutes(1));

            // Act
            await provider.ClearAllAsync();

            // Assert
            Assert.False(await provider.ExistsAsync("key1"));
            Assert.False(await provider.ExistsAsync("key2"));
        }

        /// <summary>
        /// Verifica il comportamento con cache vuota
        /// </summary>
        [Fact]
        public async Task ClearAllAsync_EmptyCache_CompletesSuccessfully()
        {
            // Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());
            var logger = Mock.Of<ILogger<MemoryCacheProvider>>();
            var provider = new MemoryCacheProvider(cache, logger);

            // Act/Assert
            await provider.ClearAllAsync(); // Non dovrebbe lanciare eccezioni
        }
    }
}