using FlexCore.Caching.Common.Validators;
using Xunit;

namespace FlexCore.Caching.Common.Tests
{
    /// <summary>
    /// Test per garantire la copertura completa della classe CacheKeyValidator
    /// </summary>
    public class CacheKeyValidatorFullCoverageTests
    {
        /// <summary>
        /// Verifica chiavi valide con caratteri permessi
        /// </summary>
        [Theory]
        [InlineData("valid_key-123")]
        [InlineData("TEST_KEY")]
        [InlineData("a")]
        [InlineData("key_with_underscore")]
        [InlineData("12345")]
        public void ValidateKey_ValidKeys_ShouldReturnTrue(string key)
        {
            // Act
            bool result = CacheKeyValidator.ValidateKey(key);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Verifica chiavi non valide con caratteri speciali
        /// </summary>
        [Theory]
        [InlineData("invalid!key")]
        [InlineData("key with spaces")]
        [InlineData("key?test")]
        [InlineData("key/with/slash")]
        public void ValidateKey_InvalidCharacters_ShouldReturnFalse(string key)
        {
            // Act
            bool result = CacheKeyValidator.ValidateKey(key);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Verifica il limite massimo di 128 caratteri
        /// </summary>
        [Fact]
        public void ValidateKey_MaxLengthBoundary_ShouldReturnTrue()
        {
            // Arrange
            var key = new string('a', 128); // Limite esatto

            // Act
            bool result = CacheKeyValidator.ValidateKey(key);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Verifica chiave vuota
        /// </summary>
        [Fact]
        public void ValidateKey_EmptyKey_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => CacheKeyValidator.ValidateKey(null!)
            );
        }
    }
}