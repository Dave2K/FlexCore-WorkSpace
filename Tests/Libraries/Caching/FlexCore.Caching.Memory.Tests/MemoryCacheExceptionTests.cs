using FlexCore.Caching.Common.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;

namespace FlexCore.Caching.Memory.Tests
{
    /// <summary>
    /// Test per la classe <see cref="MemoryCacheException"/>
    /// </summary>
    public class MemoryCacheExceptionTests
    {
        /// <summary>
        /// Verifica che venga lanciata una <see cref="ArgumentException"/> per messaggi non validi
        /// </summary>
        /// <param name="invalidMessage">Messaggio non valido da testare</param>
        [Theory]
        // Sostituito null con casi di test validi per stringhe non nullable
        [InlineData("")]         // Stringa vuota
        [InlineData("   ")]      // Spazi bianchi
        public void Constructor_InvalidMessage_ShouldThrow(string invalidMessage)
        {
            // Arrange
            var loggerMock = new Mock<ILogger<MemoryCacheException>>();
            var innerEx = new Exception("Errore interno");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new MemoryCacheException(loggerMock.Object, invalidMessage, innerEx));

            Assert.Equal("message", ex.ParamName);
        }
    }
}