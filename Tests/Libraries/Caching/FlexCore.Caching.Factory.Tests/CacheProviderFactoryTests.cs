using Xunit;
using Moq;
using FlexCore.Caching.Factory;
using FlexCore.Caching.Core.Interfaces;

namespace FlexCore.Caching.Factory.Tests
{
    public class CacheProviderFactoryTests
    {
        [Fact]
        public void CreateCacheProvider_RegisteredProvider_ReturnsInstance()
        {
            var factory = new CacheProviderFactory();
            var mockProvider = new Mock<ICacheProvider>();

            factory.RegisterProvider("memory", () => mockProvider.Object);

            var provider = factory.CreateCacheProvider("memory");
            Assert.NotNull(provider);
        }
    }
}