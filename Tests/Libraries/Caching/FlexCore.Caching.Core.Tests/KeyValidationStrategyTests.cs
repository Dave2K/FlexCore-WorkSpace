using FlexCore.Caching.Common.Validators;
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

namespace FlexCore.Caching.Core.Tests
{
    /// <summary>
    /// Verifica la strategia centralizzata di validazione delle chiavi di cache.
    /// </summary>
    public class KeyValidationStrategyTests
    {
        /// <summary>
        /// Verifica che i provider utilizzino il validatore centralizzato per le chiavi.
        /// </summary>
        /// <param name="providerType">Tipo del provider da testare (MemoryCacheProvider/RedisCacheProvider).</param>
        [Theory]
        [InlineData(typeof(MemoryCacheProvider))]
        [InlineData(typeof(RedisCacheProvider))]
        public async Task Providers_ShouldUseCentralValidatorAsync(Type providerType)
        {
            // Arrange
            var provider = CreateProvider(providerType);
            const string invalidKey = "invalid key!";

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>( // ✅ Rimosso ConfigureAwait
                () => provider.ExistsAsync(invalidKey)
            );

            Assert.Contains("CacheKeyValidator", ex.Message);
        }

        /// <summary>
        /// Verifica che BaseCacheManager non implementi logiche autonome di validazione.
        /// </summary>
        [Fact]
        public void BaseCacheManager_ShouldNotImplementCustomValidation()
        {
            // Act
            var managerMethods = typeof(BaseCacheManager).GetMethods();
            var validationMethods = Array.FindAll(
                managerMethods,
                m => m.Name.Contains("ValidateKey", StringComparison.Ordinal)
            );

            // Assert
            Assert.Empty(validationMethods);
        }

        /// <summary>
        /// Verifica il comportamento del validatore centrale con diversi tipi di chiavi.
        /// </summary>
        /// <param name="key">Chiave da validare.</param>
        /// <param name="isValid">Indica se la chiave è attesa come valida.</param>
        [Theory]
        [InlineData("ValidKey", true)]
        [InlineData("invalid key!", false)]
        [InlineData("", false)]
        [InlineData(null!, false)]
        public void CentralValidator_ShouldHandleAllCases(string? key, bool isValid)
        {
            if (isValid)
            {
                CacheKeyValidator.ThrowIfInvalid(key!);
            }
            else
            {
                Assert.Throws<ArgumentException>(() => CacheKeyValidator.ThrowIfInvalid(key!));
            }
        }

        /// <summary>
        /// Crea un'istanza del provider specificato per i test.
        /// </summary>
        /// <param name="providerType">Tipo del provider (MemoryCacheProvider/RedisCacheProvider).</param>
        /// <returns>Istanza configurata del provider.</returns>
        private ICacheProvider CreateProvider(Type providerType)
        {
            if (providerType == typeof(MemoryCacheProvider))
            {
                return new MemoryCacheProvider(
                    new MemoryCache(new MemoryCacheOptions()),
                    Mock.Of<ILogger<MemoryCacheProvider>>()
                );
            }

            if (providerType == typeof(RedisCacheProvider))
            {
                var mockMultiplexer = new Mock<IConnectionMultiplexer>();
                mockMultiplexer.Setup(m => m.GetDatabase(
                    It.IsAny<int>(),
                    It.IsAny<object>()
                )).Returns(Mock.Of<IDatabase>());

                return new RedisCacheProvider(
                    "localhost:6379",
                    Mock.Of<ILogger<RedisCacheProvider>>(),
                    mockMultiplexer.Object
                );
            }

            throw new NotSupportedException($"Provider non supportato: {providerType.Name}");
        }
    }
}