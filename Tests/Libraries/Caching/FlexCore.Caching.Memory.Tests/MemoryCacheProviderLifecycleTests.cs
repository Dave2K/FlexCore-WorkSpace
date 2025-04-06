using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FlexCore.Caching.Memory.Tests
{
    /// <summary>
    /// Test suite per il ciclo di vita di MemoryCacheProvider
    /// </summary>
    public class MemoryCacheProviderLifecycleTests
    {
        /// <summary>
        /// Verifica l'iniezione delle dipendenze
        /// </summary>
        [Fact]
        public void Constructor_ShouldAcceptValidDependencies()
        {
            // Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());
            var loggerMock = new Mock<ILogger<MemoryCacheProvider>>();

            // Act & Assert
            var provider = new MemoryCacheProvider(cache, loggerMock.Object);
            Assert.NotNull(provider);
        }

        /// <summary>
        /// Verifica il comportamento con cache null
        /// </summary>
        [Fact]
        public void Constructor_NullCache_ShouldThrow()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<MemoryCacheProvider>>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new MemoryCacheProvider(null!, loggerMock.Object));
        }
    }
}