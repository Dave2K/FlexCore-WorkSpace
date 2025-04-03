using Xunit;
using Moq;
using FlexCore.Caching.Core.Interfaces;

namespace FlexCore.Caching.Interfaces.Tests
{
    public class CacheInterfacesTests
    {
        [Fact]
        public void ICacheProvider_ShouldContainRequiredMethods()
        {
            var mockProvider = new Mock<ICacheProvider>();

            mockProvider.Setup(p => p.ExistsAsync(It.IsAny<string>()));
            mockProvider.Setup(p => p.GetAsync<string>(It.IsAny<string>()));

            Assert.NotNull(mockProvider.Object);
        }
    }
}