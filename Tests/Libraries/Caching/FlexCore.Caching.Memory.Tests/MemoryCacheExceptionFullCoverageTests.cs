using FlexCore.Caching.Common.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace FlexCore.Caching.Memory.Tests
{
    /// <summary>
    /// Test per garantire la copertura completa della classe MemoryCacheException
    /// </summary>
    public class MemoryCacheExceptionFullCoverageTests
    {
        /// <summary>
        /// Verifica il costruttore primario con logging integrato
        /// </summary>
        [Fact]
        public void FullConstructor_WithLogger_ShouldLogAndSetProperties()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<MemoryCacheException>>();
            var innerEx = new InvalidOperationException("Memoria insufficiente");
            const string message = "Allocazione fallita";

            // Act
            var ex = new MemoryCacheException(loggerMock.Object, message, innerEx);

            // Assert
            Assert.Equal(message, ex.Message);
            Assert.Same(innerEx, ex.InnerException);
            loggerMock.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(message)),
                innerEx,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        /// <summary>
        /// Verifica il costruttore secondario senza logger
        /// </summary>
        [Fact]
        public void MinimalConstructor_WithoutLogger_ShouldSetProperties()
        {
            // Arrange
            var innerEx = new OutOfMemoryException();
            const string message = "Errore critico di memoria";

            // Act
            var ex = new MemoryCacheException(message, innerEx);

            // Assert
            Assert.Equal(message, ex.Message);
            Assert.Same(innerEx, ex.InnerException);
        }

        /// <summary>
        /// Verifica il lancio di eccezione per messaggio nullo
        /// </summary>
        [Fact]
        public void Constructor_NullMessage_ShouldThrowArgumentException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<MemoryCacheException>>();
            var validEx = new Exception();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new MemoryCacheException(loggerMock.Object, null!, validEx));

            Assert.Equal("message", ex.ParamName);
        }

        /// <summary>
        /// Verifica il lancio di eccezione per messaggio vuoto
        /// </summary>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_EmptyMessage_ShouldThrowArgumentException(string invalidMessage)
        {
            // Arrange
            var loggerMock = new Mock<ILogger<MemoryCacheException>>();
            var validEx = new Exception();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new MemoryCacheException(loggerMock.Object, invalidMessage, validEx));

            Assert.Equal("message", ex.ParamName);
        }

        /// <summary>
        /// Verifica la proprietà FullStackTrace
        /// </summary>
        [Fact]
        public void FullStackTrace_ShouldCombineCurrentAndInnerStackTraces()
        {
            // Arrange
            var innerEx = new InvalidOperationException("Operazione non valida");
            var ex = new MemoryCacheException("Errore composto", innerEx);

            // Act
            var stackTrace = ex.FullStackTrace;

            // Assert
            Assert.Contains("Stack corrente:", stackTrace);
            Assert.Contains("Stack interno:", stackTrace);
            Assert.Contains("InvalidOperationException", stackTrace);
        }
    }
}