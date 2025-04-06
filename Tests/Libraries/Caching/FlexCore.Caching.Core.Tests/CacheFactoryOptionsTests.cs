using Microsoft.Extensions.Options;
using Xunit;
using FlexCore.Caching.Factory; // Aggiungere il namespace mancante

namespace FlexCore.Caching.Factory.Tests
{
    /// <summary>
    /// Test per la validazione delle opzioni del factory
    /// </summary>
    public class CacheFactoryOptionsValidationTests
    {
        /// <summary>
        /// Verifica che la validazione fallisca se ValidateProvidersOnStartup è disabilitato
        /// </summary>
        [Fact]
        public void ValidateOptions_InvalidSettings_ShouldFailValidation()
        {
            // Arrange
            var options = new CacheFactoryOptions
            {
                ValidateProvidersOnStartup = false
            };

            var validator = new OptionsValidator();

            // Act
            var result = validator.Validate(name: null!, options); // Usare null! per compatibilità con l'interfaccia

            // Assert
            Assert.False(result.Succeeded);
        }

        // Implementazione corretta con supporto nullable
        private class OptionsValidator : IValidateOptions<CacheFactoryOptions>
        {
            public ValidateOptionsResult Validate(string? name, CacheFactoryOptions options)
                => options.ValidateProvidersOnStartup
                    ? ValidateOptionsResult.Success
                    : ValidateOptionsResult.Fail("Validation failed");
        }
    }

    /// <summary>
    /// Opzioni di configurazione per il cache factory (Classe mancante)
    /// </summary>
    public class CacheFactoryOptions
    {
        public bool ValidateProvidersOnStartup { get; set; } = true;
    }
}