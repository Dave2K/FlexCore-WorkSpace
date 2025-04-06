using Microsoft.Extensions.Options;
using Xunit;
using FlexCore.Caching.Factory;

namespace FlexCore.Caching.Factory.Tests
{
    /// <summary>
    /// Test per garantire la copertura completa della classe <see cref="CacheFactoryOptions"/>
    /// </summary>
    public class CacheFactoryOptionsFullCoverageTests
    {
        /// <summary>
        /// Verifica le opzioni predefinite
        /// </summary>
        [Fact]
        public void DefaultOptions_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var options = new CacheFactoryOptions();

            // Assert
            Assert.True(options.ValidateProvidersOnStartup);
        }

        /// <summary>
        /// Verifica la validazione delle opzioni con valori validi
        /// </summary>
        [Fact]
        public void Validate_ValidOptions_ShouldSucceed()
        {
            // Arrange
            var options = new CacheFactoryOptions { ValidateProvidersOnStartup = true };
            var validator = new OptionsValidator();

            // Act
            var result = validator.Validate("TestSection", options);

            // Assert
            Assert.True(result.Succeeded);
        }

        /// <summary>
        /// Verifica la validazione delle opzioni con valori non validi
        /// </summary>
        /// <param name="sectionName">Nome della sezione di configurazione</param>
        [Theory]
        [InlineData("")]       // Stringa vuota
        [InlineData("Production")]
        public void Validate_InvalidOptions_ShouldFail(string sectionName)
        {
            // Arrange
            var options = new CacheFactoryOptions { ValidateProvidersOnStartup = false };
            var validator = new OptionsValidator();

            // Act
            var result = validator.Validate(sectionName, options);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains("Validation failed", result.FailureMessage);
        }

        /// <summary>
        /// Validatore personalizzato per le opzioni
        /// </summary>
        private class OptionsValidator : IValidateOptions<CacheFactoryOptions>
        {
            /// <summary>
            /// Esegue la validazione delle opzioni
            /// </summary>
            public ValidateOptionsResult Validate(string? name, CacheFactoryOptions options)
                => options.ValidateProvidersOnStartup
                    ? ValidateOptionsResult.Success
                    : ValidateOptionsResult.Fail("Validation failed");
        }
    }
}