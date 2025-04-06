using FlexCore.Caching.Core.Interfaces;
using FlexCore.Caching.Factory;
using Moq;
using System;
using Xunit;

namespace FlexCore.Caching.Factory.Tests
{
    public class CacheFactoryOptionsValidationTests
    {
        [Fact]
        public void ValidateProviders_WithInvalidProvider_ThrowsException()
        {
            // Arrange  
            var factoryMock = new Mock<ICacheFactory>();
            factoryMock.Setup(f => f.GetRegisteredProviders()) // 👈 Metodo corretto  
                      .Returns(new[] { "FaultyProvider" });

            factoryMock.Setup(f => f.CreateCacheProvider("FaultyProvider"))
                      .Throws(new Exception("Simulated error"));

            var options = new CacheFactoryOptions();

            // Act & Assert  
            var ex = Assert.Throws<InvalidOperationException>(
                () => options.ValidateProviders(factoryMock.Object)
            );

            Assert.Contains("FaultyProvider", ex.Message);
            Assert.Contains("Simulated error", ex.Message);
        }

        [Fact]
        public void ValidateProviders_WithNoProviders_ThrowsException()
        {
            // Arrange  
            var factoryMock = new Mock<ICacheFactory>();
            factoryMock.Setup(f => f.GetRegisteredProviders())
                      .Returns(Array.Empty<string>());

            var options = new CacheFactoryOptions();

            // Act & Assert  
            Assert.Throws<InvalidOperationException>(
                () => options.ValidateProviders(factoryMock.Object)
            );
        }
    }
}