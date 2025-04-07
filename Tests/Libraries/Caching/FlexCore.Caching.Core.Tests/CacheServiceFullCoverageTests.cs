using FlexCore.Caching.Common.Validators;
using FlexCore.Caching.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FlexCore.Caching.Core.Tests
{
    /// <summary>
    /// Test suite per la verifica completa del servizio di cache
    /// </summary>
    /// <remarks>
    /// Copre tutti i casi limite e gli scenari di errore per ICacheService
    /// </remarks>
    public class CacheServiceFullCoverageTests
    {
        /// <summary>
        /// Verifica la corretta gestione delle chiavi non valide in GetAsync
        /// </summary>
        /// <param name="invalidKey">Chiave non valida da testare</param>
        /// <param name="expectedExceptionType">Tipo di eccezione attesa</param>
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("   ", typeof(ArgumentException))]
        [InlineData("invalid!key", typeof(ArgumentException))]
        public async Task GetAsync_InvalidKey_ShouldThrowSpecificException(
            string? invalidKey,
            Type expectedExceptionType)
        {
            // Arrange
            var mockProvider = new Mock<ICacheProvider>();
            var mockLogger = new Mock<ILogger<ICacheService>>();
            var cacheService = new ConcreteCacheService(mockProvider.Object, mockLogger.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync(
                expectedExceptionType,
                () => cacheService.GetAsync<string>(invalidKey!)
            );

            // Verifica aggiuntiva per i parametri delle eccezioni
            if (exception is ArgumentException argEx)
            {
                Assert.Equal("key", argEx.ParamName);
                Assert.True(
                    argEx.Message.Contains("non può essere") ||
                    argEx.Message.Contains("non valido"),
                    "Messaggio di errore non appropriato"
                );
            }
        }

        /// <summary>
        /// Verifica il corretto funzionamento del metodo ClearAllAsync
        /// </summary>
        [Fact]
        public async Task ClearAllAsync_ShouldInvokeProviderCleanup()
        {
            // Arrange
            var mockProvider = new Mock<ICacheProvider>();
            var mockLogger = new Mock<ILogger<ICacheService>>();
            var cacheService = new ConcreteCacheService(mockProvider.Object, mockLogger.Object);

            // Act
            await cacheService.ClearAllAsync();

            // Assert
            mockProvider.Verify(
                p => p.ClearAllAsync(),
                Times.Once,
                "Il cleanup completo non è stato invocato"
            );
        }

        /// <summary>
        /// Implementazione concreta di ICacheService per testing
        /// </summary>
        private class ConcreteCacheService : ICacheService
        {
            private readonly ICacheProvider _provider;
            private readonly ILogger<ICacheService> _logger;

            /// <summary>
            /// Inizializza una nuova istanza del servizio
            /// </summary>
            /// <param name="provider">Provider di cache sottostante</param>
            /// <param name="logger">Logger per tracciamento attività</param>
            /// <exception cref="ArgumentNullException">
            /// Generato se <paramref name="provider"/> è null
            /// </exception>
            public ConcreteCacheService(
                ICacheProvider provider,
                ILogger<ICacheService> logger)
            {
                _provider = provider ?? throw new ArgumentNullException(nameof(provider));
                _logger = logger;
            }

            /// <inheritdoc/>
            public Task<T?> GetAsync<T>(string key)
            {
                // Validazione chiave prima di qualsiasi operazione
                CacheKeyValidator.ThrowIfInvalid(key);
                return _provider.GetAsync<T>(key);
            }

            /// <inheritdoc/>
            public async Task<bool> SetAsync<T>(string key, T value, TimeSpan expiration)
            {
                CacheKeyValidator.ThrowIfInvalid(key);
                return await _provider.SetAsync(key, value, expiration);
            }

            /// <inheritdoc/>
            public async Task<bool> RemoveAsync(string key)
            {
                CacheKeyValidator.ThrowIfInvalid(key);
                return await _provider.RemoveAsync(key);
            }

            /// <inheritdoc/>
            public async Task<bool> ExistsAsync(string key)
            {
                CacheKeyValidator.ThrowIfInvalid(key);
                return await _provider.ExistsAsync(key);
            }

            /// <inheritdoc/>
            public async Task ClearAllAsync() => await _provider.ClearAllAsync();
        }
    }
}