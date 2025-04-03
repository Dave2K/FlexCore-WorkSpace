using FlexCore.Caching.Core;
using FlexCore.Caching.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FlexCore.Caching.Core.Tests
{
    public class BaseCacheManagerTests
    {
        // ✅ Classe pubblica
        public class TestCacheManager : BaseCacheManager
        {
            public TestCacheManager(ILogger logger, ICacheProvider cacheProvider)
                : base(logger, cacheProvider) { }
        }

        [Fact]
        public void Exists_CallsProviderExists()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var cacheProviderMock = new Mock<ICacheProvider>();
            var manager = new TestCacheManager(loggerMock.Object, cacheProviderMock.Object);

            // Act
            manager.ExistsAsync("test_key");

            // Assert
            cacheProviderMock.Verify(p => p.ExistsAsync("test_key"), Times.Once);
        }
    }
}