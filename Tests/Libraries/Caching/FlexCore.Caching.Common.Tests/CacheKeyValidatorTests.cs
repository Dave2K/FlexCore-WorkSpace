using Xunit;
using FlexCore.Caching.Common.Validators;

namespace FlexCore.Caching.Common.Tests
{
    public class CacheKeyValidatorTests
    {
        [Theory]
        [InlineData("valid_key")]
        public void ValidateKey_ValidKey_ReturnsTrue(string key)
        {
            // Rimuovi l'istanza (la classe è statica)
            bool result = CacheKeyValidator.ValidateKey(key);
            Assert.True(result);
        }
    }
}