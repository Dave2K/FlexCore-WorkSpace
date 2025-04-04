using Xunit;
using Moq;
using FlexCore.Caching.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace FlexCore.Caching.Core.Tests
{
    /// <summary>
    /// Test per la classe <see cref="BaseCacheManager"/>
    /// </summary>
    public class BaseCacheManagerTests
    {
        private class TestCacheManager : BaseCacheManager
        {
            public TestCacheManager(ILogger logger, ICacheProvider provider)
                : base(logger, provider) { }
        }

        /// <summary>
        /// Verifica che il costruttore sollevi eccezioni per parametri nulli
        /// </summary>
        [Fact]
        public void Constructor_NullParameters_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new TestCacheManager(null!, Mock.Of<ICacheProvider>()));

            Assert.Throws<ArgumentNullException>(() =>
                new TestCacheManager(Mock.Of<ILogger>(), null!));
        }

        /// <summary>
        /// Verifica il corretto funzionamento del metodo ExistsAsync
        /// </summary>
        [Fact]
        public async Task ExistsAsync_ValidKey_CallsProvider()
        {
            var providerMock = new Mock<ICacheProvider>();
            var manager = new TestCacheManager(
                Mock.Of<ILogger>(),
                providerMock.Object
            );

            await manager.ExistsAsync("valid_key");
            providerMock.Verify(p => p.ExistsAsync("valid_key"), Times.Once);
        }

        /// <summary>
        /// Verifica il comportamento con chiavi non valide
        /// </summary>
        /// <param name="key">Chiave da testare</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ValidateKey_InvalidKeys_ThrowsException(string? key)
        {
            var manager = new TestCacheManager(
                Mock.Of<ILogger>(),
                Mock.Of<ICacheProvider>()
            );

            Assert.Throws<ArgumentException>(() =>
                manager.ExistsAsync(key!).GetAwaiter().GetResult());
        }
    }
}