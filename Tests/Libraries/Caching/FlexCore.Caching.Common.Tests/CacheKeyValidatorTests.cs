using Xunit;
using FlexCore.Caching.Common.Validators;
using System;

namespace FlexCore.Caching.Common.Tests
{
    /// <summary>
    /// Test suite per la classe <see cref="CacheKeyValidator"/>
    /// </summary>
    public class CacheKeyValidatorTests
    {
        /// <summary>
        /// Verifica il formato massimo consentito (128 caratteri)
        /// </summary>
        [Fact]
        public void ValidateKey_MaxLengthKey_ShouldReturnTrue()
        {
            // Arrange
            var key = new string('a', 128);

            // Act
            bool result = CacheKeyValidator.ValidateKey(key);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Verifica caratteri speciali non consentiti
        /// </summary>
        [Theory]
        [InlineData("key!")]
        [InlineData("key?test")]
        [InlineData("key with spaces")]
        public void ValidateKey_InvalidCharacters_ShouldReturnFalse(string key)
        {
            // Act
            bool result = CacheKeyValidator.ValidateKey(key);

            // Assert
            Assert.False(result);
        }
    }
}