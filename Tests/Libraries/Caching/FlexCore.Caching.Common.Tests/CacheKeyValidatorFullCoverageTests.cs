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
            CacheKeyValidator.ThrowIfInvalid(key); // ✅ Sostituito ValidateKey
        }

        [Theory]
        [InlineData("invalid!key")]
        [InlineData("key with spaces")]
        [InlineData("key?test")]
        [InlineData("key/with/slash")]
        public void ValidateKey_InvalidCharacters_ShouldThrow(string key)
        {
            Assert.Throws<ArgumentException>(() =>
                CacheKeyValidator.ThrowIfInvalid(key)); // ✅ Sostituito ValidateKey
        }

        [Fact]
        public void ValidateKey_MaxLengthBoundary_ShouldNotThrow()
        {
            var key = new string('a', 128);
            CacheKeyValidator.ThrowIfInvalid(key); // ✅ Sostituito ValidateKey
        }

        [Fact]
        public void ValidateKey_EmptyKey_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() =>
                CacheKeyValidator.ThrowIfInvalid(null!)); // ✅ Sostituito ValidateKey
        }
    }
}