using FlexCore.Caching.Common.Validators;
using Xunit;
using System;

namespace FlexCore.Caching.Core.Tests
{
    /// <summary>
    /// Test suite per la validazione delle chiavi in BaseCacheManager
    /// </summary>
    public class BaseCacheManagerTests
    {
        /// <summary>
        /// Verifica che chiavi non valide generino eccezioni specifiche
        /// </summary>
        /// <param name="key">Chiave da testare</param>
        /// <param name="expectedException">Tipo di eccezione attesa</param>
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("  ", typeof(ArgumentException))]
        [InlineData("invalid!key", typeof(ArgumentException))]
        public void ValidateKey_InvalidKeys_ThrowsSpecificExceptions(
            string? key,
            Type expectedException)
        {
            var exception = Assert.Throws(
                expectedException,
                () => CacheKeyValidator.ThrowIfInvalid(key!)
            );

            if (exception is ArgumentException argEx)
            {
                Assert.Equal("key", argEx.ParamName);
            }
        }

        /// <summary>
        /// Verifica che chiavi valide non generino eccezioni
        /// </summary>
        /// <param name="key">Chiave valida da testare</param>
        [Theory]
        [InlineData("valid_key-123")]
        [InlineData("TEST_KEY")]
        [InlineData("a")]
        public void ValidateKey_ValidKeys_ShouldNotThrow(string key)
        {
            CacheKeyValidator.ThrowIfInvalid(key);
        }
    }
}