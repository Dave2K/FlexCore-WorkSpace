using Xunit;
using Moq;
using FlexCore.Caching.Core;
using FlexCore.Caching.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlexCore.Caching.Core.Tests
{
    /// <summary>
    /// Test per la classe BaseCacheManager
    /// </summary>
    public class BaseCacheManagerTests
    {
        private class TestCacheManager(ILogger<TestCacheManager> logger, ICacheProvider provider)
            : BaseCacheManager(logger, provider)
        { }

        /// <summary>
        /// Verifica che il metodo Exists richiami correttamente il provider
        /// </summary>
        [Fact]
        public void Exists_CallsProviderExists()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<TestCacheManager>>();
            var providerMock = new Mock<ICacheProvider>();
            var manager = new TestCacheManager(loggerMock.Object, providerMock.Object);

            // Act
            _ = manager.Exists("test_key");

            // Assert
            providerMock.Verify(p => p.Exists("test_key"), Times.Once);
        }
    }
}