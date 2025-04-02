using Xunit;
using Moq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using FlexCore.Caching.Memory;

namespace FlexCore.Caching.Memory.Tests
{
    public class MemoryCacheProviderTests
    {
        private readonly MemoryCacheProvider _provider;
        private readonly Mock<ILogger<MemoryCacheProvider>> _loggerMock = new();

        public MemoryCacheProviderTests()
        {
            _provider = new MemoryCacheProvider(
                new MemoryCache(new MemoryCacheOptions()),
                _loggerMock.Object
            );
        }

        [Fact]
        public void SetAndGet_ValidKey_ReturnsValue()
        {
            _provider.Set("testKey", "testValue", TimeSpan.FromMinutes(1));
            var result = _provider.Get<string>("testKey");

            Assert.Equal("testValue", result);
        }
    }
}