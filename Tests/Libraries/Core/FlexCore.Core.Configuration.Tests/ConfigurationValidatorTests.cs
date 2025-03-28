using FlexCore.Core.Configuration.Validators;
using Xunit;

namespace FlexCore.Core.Configuration.Tests
{
    public class ConfigurationValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ValidateKey_InvalidKey_ThrowsException(string? key)
        {
            Assert.ThrowsAny<ArgumentException>(() => ConfigurationValidator.ValidateKey(key!));
        }

        [Fact]
        public void ValidateKey_ValidKey_NoException()
        {
            ConfigurationValidator.ValidateKey("ValidKey");
        }
    }
}