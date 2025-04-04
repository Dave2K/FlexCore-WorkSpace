using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using FlexCore.Caching.Common.Exceptions;
using System;

namespace FlexCore.Caching.Common.Tests
{
    /// <summary>
    /// Test suite per la classe <see cref="CacheExceptionHandler"/>
    /// </summary>
    public class CacheExceptionHandlerTests
    {
        /// <summary>
        /// Verifica la creazione corretta di un'eccezione tipizzata con contesto
        /// </summary>
        [Fact]
        public void HandleException_WithValidParameters_ShouldReturnTypedException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var innerEx = new Exception("Inner error");
            const string operation = "cache operation";
            const string key = "item123";

            // Act
            var result = CacheExceptionHandler.HandleException<CacheException>(
                loggerMock.Object,
                innerEx,
                operation,
                key);

            // Assert
            Assert.Contains(operation, result.Message);
            Assert.Contains(key, result.Message);
            Assert.IsType<CacheException>(result);
        }

        /// <summary>
        /// Verifica il comportamento con logger nullo
        /// </summary>
        [Fact]
        public void HandleException_WithNullLogger_ShouldThrow()
        {
            // Arrange
            var innerEx = new Exception();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                CacheExceptionHandler.HandleException<CacheException>(
                    null!,
                    innerEx,
                    "operation"));
        }
    }
}