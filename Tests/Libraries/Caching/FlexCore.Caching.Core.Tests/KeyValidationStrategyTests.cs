using FlexCore.Caching.Common.Validators;
using Xunit;
using System;

namespace FlexCore.Caching.Core.Tests
{
    /// <summary>
    /// Test suite per la strategia centralizzata di validazione delle chiavi
    /// </summary>
    /// <remarks>
    /// Verifica tutte le condizioni di errore e i messaggi associati
    /// </remarks>
    public class KeyValidationStrategyTests
    {
        /// <summary>
        /// Test parametrizzato per la validazione delle chiavi
        /// </summary>
        /// <param name="key">Chiave da testare</param>
        /// <param name="expectedExceptionType">Tipo di eccezione attesa</param>
        [Theory]
        [InlineData("ValidKey-123", null)]
        [InlineData("invalid!key", typeof(ArgumentException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("   ", typeof(ArgumentException))]
        [InlineData(null, typeof(ArgumentNullException))]
        public void CentralValidator_ShouldHandleAllCases(
            string? key,
            Type? expectedExceptionType)
        {
            // Arrange
            Action testAction = () => CacheKeyValidator.ThrowIfInvalid(key!);

            // Act & Assert
            if (expectedExceptionType == null)
            {
                testAction();
            }
            else if (expectedExceptionType == typeof(ArgumentNullException))
            {
                var ex = Assert.Throws<ArgumentNullException>(testAction);
                Assert.Equal("key", ex.ParamName);
                Assert.Contains("non può essere null", ex.Message);
            }
            else
            {
                var ex = Assert.Throws<ArgumentException>(testAction);
                Assert.Equal("key", ex.ParamName);

                if (string.IsNullOrWhiteSpace(key))
                {
                    Assert.Contains("non può essere vuota o contenere solo spazi", ex.Message);
                }
                else
                {
                    Assert.Contains("non valido", ex.Message);
                }
            }
        }

        /// <summary>
        /// Verifica il comportamento con chiave di lunghezza massima
        /// </summary>
        [Fact]
        public void MaxLengthKey_ShouldBeValid()
        {
            // Arrange
            var key = new string('a', 128);

            // Act & Assert
            CacheKeyValidator.ThrowIfInvalid(key);
        }

        /// <summary>
        /// Verifica il comportamento con chiave troppo lunga
        /// </summary>
        [Fact]
        public void OverMaxLengthKey_ShouldThrow()
        {
            // Arrange
            var key = new string('a', 129);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(
                () => CacheKeyValidator.ThrowIfInvalid(key)
            );

            Assert.Equal("key", ex.ParamName);
            Assert.Contains("non valido", ex.Message);
        }

        /// <summary>
        /// Verifica il comportamento con caratteri speciali non consentiti
        /// </summary>
        [Theory]
        [InlineData("key?test")]
        [InlineData("test/key")]
        [InlineData("key@test")]
        public void InvalidCharacters_ShouldThrow(string invalidKey)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(
                () => CacheKeyValidator.ThrowIfInvalid(invalidKey)
            );

            Assert.Equal("key", ex.ParamName);
            Assert.Contains("non valido", ex.Message);
        }
    }
}