using Xunit;
using FlexCore.Caching.Common.Validators;
using System;

namespace FlexCore.Caching.Common.Tests
{
    public class CacheKeyValidatorTests
    {
        [Fact]
        public void ValidateKey_MaxLengthKey_ShouldNotThrow()
        {
            // Arrange
            var key = new string('a', 128);

            // Act & Assert
            CacheKeyValidator.ThrowIfInvalid(key); // ✅ Sostituito ValidateKey
        }

        [Theory]
        [InlineData("key!")]
        [InlineData("key?test")]
        [InlineData("key with spaces")]
        public void ValidateKey_InvalidCharacters_ShouldThrow(string key)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                CacheKeyValidator.ThrowIfInvalid(key)); // ✅ Sostituito ValidateKey
        }
    }
}