using FlexCore.Caching.Common.Validators;
using Xunit;
using System;

namespace FlexCore.Caching.Common.Tests
{
    public class CacheKeyValidatorFullCoverageTests
    {
        [Theory]
        [InlineData("valid_key-123")]
        [InlineData("TEST_KEY")]
        [InlineData("a")]
        [InlineData("key_with_underscore")]
        [InlineData("12345")]
        public void ValidateKey_ValidKeys_ShouldNotThrow(string key)
        {
            // Act & Assert
            CacheKeyValidator.ValidateKey(key); // Nessuna eccezione = test passa
        }

        [Theory]
        [InlineData("invalid!key")]
        [InlineData("key with spaces")]
        [InlineData("key?test")]
        [InlineData("key/with/slash")]
        public void ValidateKey_InvalidCharacters_ShouldThrow(string key)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => CacheKeyValidator.ValidateKey(key));
        }

        [Fact]
        public void ValidateKey_MaxLengthBoundary_ShouldNotThrow()
        {
            // Arrange
            var key = new string('a', 128);

            // Act & Assert
            CacheKeyValidator.ValidateKey(key); // Nessuna eccezione
        }

        [Fact]
        public void ValidateKey_EmptyKey_ShouldThrow()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => CacheKeyValidator.ValidateKey(null!));
        }
    }
}