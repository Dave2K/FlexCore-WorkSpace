using FlexCore.Caching.Common.Validators;
using FlexCore.Caching.Core;
using FlexCore.Caching.Core.Interfaces;
using FlexCore.Caching.Memory;
using FlexCore.Caching.Redis;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;
using System;
using Xunit;

namespace FlexCore.Caching.Core.Tests
{
    /// <summary>
    /// Verifica che la validazione delle chiavi sia centralizzata e applicata coerentemente
    /// </summary>
    public class KeyValidationStrategyTests
    {
        /// <summary>
        /// Verifica che TUTTI i provider utilizzino esclusivamente CacheKeyValidator
        /// </summary>
        [Theory]
        [InlineData(typeof(MemoryCacheProvider))]
        [InlineData(typeof(RedisCacheProvider))]
        public void Providers_ShouldUseOnlyCentralValidator(Type providerType)
        {
            // Arrange
            var provider = CreateProvider(providerType);
            var invalidKey = "invalid key!";

            // Act/Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(
                () => provider.ExistsAsync(invalidKey)
            ).Result;

            Assert.Contains("CacheKeyValidator", ex.Message);
        }

        /// <summary>
        /// Verifica che BaseCacheManager non esegua validazioni autonome
        /// </summary>
        [Fact]
        public void BaseCacheManager_ShouldNotImplementCustomValidation()
        {
            // Arrange
            var managerMethods = typeof(BaseCacheManager).GetMethods();

            // Act
            var validationMethods = managerMethods
                .Where(m => m.Name.Contains("ValidateKey"))
                .ToList();

            // Assert
            Assert.Empty(validationMethods);
        }

        /// <summary>
        /// Verifica il pattern di validazione centralizzato su tutti i metodi
        /// </summary>
        [Theory]
        [InlineData("ValidKey", true)]
        [InlineData("invalid key!", false)]
        [InlineData("", false)]
        [InlineData(null!, false)]
        public void CentralValidator_ShouldHandleAllCases(string key, bool isValid)
        {
            // Act/Assert
            if (isValid)
            {
                CacheKeyValidator.ValidateKey(key);
                Assert.True(true);
            }
            else
            {
                Assert.Throws<ArgumentException>(
                    () => CacheKeyValidator.ValidateKey(key)
                );
            }
        }

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

            throw new NotSupportedException();
        }
    }
}