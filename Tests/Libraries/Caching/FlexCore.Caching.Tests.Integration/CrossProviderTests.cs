using FlexCore.Caching.Core.Interfaces;
using FlexCore.Caching.Memory;
using FlexCore.Caching.Redis;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FlexCore.Caching.Tests.Integration
{
    /// <summary>
    /// Test di integrazione avanzata per verificare il funzionamento cross-provider
    /// </summary>
    public class CrossProviderTests : IDisposable
    {
        private ICacheProvider _provider; // Rimosso 'readonly'
        private IDisposable? _disposableProvider; // Rimosso 'readonly'

        /// <summary>
        /// Inizializza una nuova istanza della classe di test
        /// </summary>
        public CrossProviderTests()
        {
            // Spostata tutta la logica di inizializzazione nel costruttore
            var providerType = "Memory"; // O "Redis" per testare l'altro provider

            if (providerType == "Redis")
            {
                var multiplexer = ConnectionMultiplexer.Connect("localhost:6379");
                _provider = new RedisCacheProvider(
                    "localhost:6379",
                    Mock.Of<ILogger<RedisCacheProvider>>(),
                    multiplexer
                );
            }
            else
            {
                _provider = new MemoryCacheProvider(
                    new MemoryCache(new MemoryCacheOptions()),
                    Mock.Of<ILogger<MemoryCacheProvider>>()
                );
            }

            _disposableProvider = _provider as IDisposable;
        }

        /// <summary>
        /// Verifica l'inserimento e recupero di valori tra provider
        /// </summary>
        [Theory]
        [InlineData("test_key")]
        [InlineData("special_chars_!@#$%")]
        public async Task SetAndGet_ValidKey_ReturnsValue(string key)
        {
            // Arrange
            const int expectedValue = 42;

            // Act
            await _provider.SetAsync(key, expectedValue, TimeSpan.FromMinutes(1));
            var result = await _provider.GetAsync<int>(key);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        /// <summary>
        /// Esegue la pulizia delle risorse
        /// </summary>
        public void Dispose()
        {
            _disposableProvider?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}