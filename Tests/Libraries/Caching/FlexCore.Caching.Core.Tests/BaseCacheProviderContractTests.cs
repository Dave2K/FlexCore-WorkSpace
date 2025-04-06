using FlexCore.Caching.Core.Interfaces;
using Xunit;

namespace FlexCore.Caching.Core.Tests.CrossProvider
{
    /// <summary>
    /// Test suite base per verificare il contratto comune tra tutti i provider di cache
    /// </summary>
    public abstract class BaseCacheProviderContractTests
    {
        protected abstract ICacheProvider CreateProvider();

        /// <summary>
        /// Verifica che un valore salvato sia recuperabile correttamente
        /// </summary>
        [Fact]
        public async Task SetAsync_ThenGetAsync_ShouldReturnSameValue()
        {
            // Arrange
            var provider = CreateProvider();
            const string key = "contract_key1";
            const string value = "test_value";

            // Act
            await provider.SetAsync(key, value, TimeSpan.FromMinutes(1));
            var result = await provider.GetAsync<string>(key);

            // Assert
            Assert.Equal(value, result);
        }

        /// <summary>
        /// Verifica che la rimozione di una chiave la renda inaccessibile
        /// </summary>
        [Fact]
        public async Task RemoveAsync_ShouldMakeKeyUnavailable()
        {
            // Arrange
            var provider = CreateProvider();
            const string key = "contract_key2";

            // Act
            await provider.SetAsync(key, 100, TimeSpan.FromMinutes(1));
            await provider.RemoveAsync(key);
            var exists = await provider.ExistsAsync(key);

            // Assert
            Assert.False(exists);
        }

        /// <summary>
        /// Verifica che valori null siano gestiti correttamente
        /// </summary>
        [Fact]
        public async Task SetNullValue_ShouldBeRetrievableAsNull()
        {
            // Arrange
            var provider = CreateProvider();
            const string key = "contract_key3";

            // Act
            await provider.SetAsync<string>(key, null!, TimeSpan.FromMinutes(1));
            var result = await provider.GetAsync<string>(key);

            // Assert
            Assert.Null(result);
        }
    }
}