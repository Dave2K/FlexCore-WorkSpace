using FlexCore.Caching.Common.Exceptions;
using FlexCore.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace FlexCore.Caching.Memory.Tests
{
    public class MemoryCacheExceptionTests
    {
        /// <summary>
        /// Verifica che venga lanciata ArgumentException per messaggi non validi.
        /// </summary>
        [Theory]
        [InlineData("")]         // Stringa vuota
        [InlineData("   ")]      // Spazi bianchi
        public void Constructor_InvalidMessage_ShouldThrow(string invalidMessage)
        {
            // Arrange
            var loggerMock = new Mock<ILogger<MemoryCacheException>>();
            var innerEx = new Exception("Errore interno");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new MemoryCacheException(loggerMock.Object, invalidMessage, innerEx)
            );

            Assert.Equal("message", ex.ParamName);
        }
    }
}