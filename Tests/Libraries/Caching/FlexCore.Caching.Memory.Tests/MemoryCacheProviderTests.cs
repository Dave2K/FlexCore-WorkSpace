using FlexCore.Caching.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FlexCore.Caching.Memory.Tests
{
    /// <summary>
    /// Test suite per MemoryCacheProvider
    /// </summary>
    public class MemoryCacheProviderTests : IDisposable
    {
        private readonly MemoryCacheProvider _provider;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Inizializza una nuova istanza dei test
        /// </summary>
        public MemoryCacheProviderTests()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _provider = new MemoryCacheProvider(
                _cache,
                Mock.Of<ILogger<MemoryCacheProvider>>()
            );
        }

        /// <summary>
        /// Verifica che GetAsync sollevi eccezione per chiavi non valide
        /// </summary>
        [Theory]
        [InlineData(null)]    // Test esplicito per null
        [InlineData("")]      // Stringa vuota
        [InlineData("  ")]    // Spazi bianchi
        public async Task GetAsync_InvalidKey_ThrowsArgumentException(string? invalidKey)
        {
            // Usa il null-forgiving operator solo dopo aver verificato la logica
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _provider.GetAsync<string>(invalidKey!));

            Assert.Equal("key", exception.ParamName);
        }

        /// <summary>
        /// Pulizia delle risorse
        /// </summary>
        public void Dispose()
        {
            _provider.Dispose();
            _cache.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}