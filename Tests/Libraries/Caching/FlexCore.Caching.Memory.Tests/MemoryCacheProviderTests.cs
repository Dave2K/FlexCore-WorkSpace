using Xunit;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FlexCore.Caching.Memory.Tests
{
    /// <summary>
    /// Test suite per la classe <see cref="MemoryCacheProvider"/>
    /// </summary>
    public class MemoryCacheProviderTests
    {
        /// <summary>
        /// Verifica il comportamento di ClearAllAsync con cache vuota
        /// </summary>
        [Fact]
        public async Task ClearAllAsync_OnEmptyCache_ShouldNotThrow()
        {
            // Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());
            var logger = Mock.Of<ILogger<MemoryCacheProvider>>();
            var provider = new MemoryCacheProvider(cache, logger);

            // Act & Assert
            await provider.ClearAllAsync();
        }

        /// <summary>
        /// Verifica il logging degli errori durante le operazioni
        /// </summary>
        [Fact]
        public async Task SetAsync_OnError_ShouldLogError()
        {
            // Arrange
            var faultyCache = new Mock<IMemoryCache>();
            faultyCache.Setup(c => c.CreateEntry(It.IsAny<object>()))
                      .Throws(new Exception("Simulated error"));

            var loggerMock = new Mock<ILogger<MemoryCacheProvider>>();
            var provider = new MemoryCacheProvider(faultyCache.Object, loggerMock.Object);

            // Act
            await provider.SetAsync("key", "value", TimeSpan.Zero);

            // Assert
            loggerMock.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ));
        }
    }
}