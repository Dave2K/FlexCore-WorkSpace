using Xunit;
using FlexCore.Core.Configuration;

namespace FlexCore.Core.Configuration.Tests;

public class ConfigurationValidatorTests
{
    [Fact]
    public void ValidateKey_ThrowsArgumentNullException_WhenKeyIsNull()
    {
        // Arrange
        string? nullKey = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(
            () => ConfigurationValidator.ValidateKey(nullKey!));

        Assert.Equal("key", exception.ParamName);
        Assert.Contains("non può essere nulla", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void ValidateKey_ThrowsArgumentException_WhenKeyIsEmptyOrWhitespace(string invalidKey)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => ConfigurationValidator.ValidateKey(invalidKey));

        Assert.Equal("key", exception.ParamName);
        Assert.Contains("non può essere vuota", exception.Message);
    }

    [Theory]
    [InlineData("valid_key")]
    [InlineData(" test ")] // spazi laterali sono ammessi
    [InlineData("123")]
    [InlineData("a")]
    public void ValidateKey_DoesNotThrow_WhenKeyIsValid(string validKey)
    {
        // Act
        var exception = Record.Exception(
            () => ConfigurationValidator.ValidateKey(validKey));

        // Assert
        Assert.Null(exception);
    }
}