using Xunit;
using FlexCore.Caching.Common.Validators;
using System;

namespace FlexCore.Caching.Common.Tests
{
    /// <summary>
    /// Classe di test per <see cref="CacheKeyValidator"/>
    /// </summary>
    public class CacheKeyValidatorTests
    {
        /// <summary>
        /// Verifica che chiavi valide vengano accettate dal validatore
        /// </summary>
        /// <param name="key">Chiave da testare</param>
        [Theory]
        [InlineData("valid_key")]
        [InlineData("Key-With_Underscore123")]
        [InlineData("123456")]
        public void ValidateKey_ValidKeys_ReturnsTrue(string key)
        {
            // Act
            bool result = CacheKeyValidator.ValidateKey(key);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Verifica che chiavi con caratteri non consentiti vengano rifiutate
        /// </summary>
        /// <param name="key">Chiave non valida</param>
        [Theory]
        [InlineData("invalid@key")]
        [InlineData("key with space")]
        [InlineData("key#hash")]
        public void ValidateKey_InvalidCharacters_ReturnsFalse(string key)
        {
            // Act
            bool result = CacheKeyValidator.ValidateKey(key);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Verifica il comportamento con chiavi vuote o spazi bianchi
        /// </summary>
        /// <param name="key">Chiave vuota o con spazi</param>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        public void ValidateKey_EmptyOrWhitespace_ReturnsFalse(string key)
        {
            // Act
            bool result = CacheKeyValidator.ValidateKey(key);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Verifica che venga sollevata un'eccezione per chiavi null
        /// </summary>
        [Fact]
        public void ValidateKey_NullKey_ThrowsArgumentNullException()
        {
            // Arrange
            string? key = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => CacheKeyValidator.ValidateKey(key!)
            );
        }

        /// <summary>
        /// Verifica il limite massimo di lunghezza consentita (128 caratteri)
        /// </summary>
        [Fact]
        public void ValidateKey_MaxLengthKey_ReturnsTrue()
        {
            // Arrange
            string key = new string('a', 128);

            // Act
            bool result = CacheKeyValidator.ValidateKey(key);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Verifica il superamento del limite massimo di lunghezza
        /// </summary>
        [Fact]
        public void ValidateKey_ExceedMaxLength_ReturnsFalse()
        {
            // Arrange
            string key = new string('a', 129);

            // Act
            bool result = CacheKeyValidator.ValidateKey(key);

            // Assert
            Assert.False(result);
        }
    }
}