using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using FlexCore.Caching.Common.Exceptions;
using System;

namespace FlexCore.Caching.Common.Tests
{
    /// <summary>
    /// Test per <see cref="CacheExceptionHandler"/>
    /// </summary>
    public class CacheExceptionHandlerTests
    {
        /// <summary>
        /// Verifica la creazione corretta di un'eccezione tipizzata
        /// </summary>
        [Fact]
        public void HandleException_ValidType_ReturnsTypedException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var innerEx = new Exception("Inner error");

            // Act
            var result = CacheExceptionHandler.HandleException<CacheException>(
                loggerMock.Object,
                innerEx,
                "test operation",
                "testKey");

            // Assert
            Assert.IsType<CacheException>(result);
            Assert.Contains("test operation", result.Message);
            Assert.Contains("testKey", result.Message);
            Assert.Same(innerEx, result.InnerException);
        }

        /// <summary>
        /// Verifica il logging corretto dei dettagli dell'errore
        /// </summary>
        [Fact]
        public void HandleException_LogsCorrectDetails()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var innerEx = new Exception("Test error");

            // Act
            var result = CacheExceptionHandler.HandleException<CacheException>(
                loggerMock.Object,
                innerEx,
                "SET",
                "key123",
                "TestMethod");

            // Assert
            loggerMock.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("[TestMethod] Errore durante SET (Key: key123)")),
                innerEx,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        /// <summary>
        /// Verifica il comportamento con tipo di eccezione non valido
        /// </summary>
        [Fact]
        public void HandleException_InvalidExceptionType_ThrowsInvalidOperation()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() =>
                CacheExceptionHandler.HandleException<InvalidCacheException>(
                    loggerMock.Object,
                    new Exception("Test"),
                    "operation"));

            Assert.Contains("deve avere un costruttore con parametri (string, Exception)", ex.Message);
        }

        /// <summary>
        /// Verifica la gestione di parametri nulli
        /// </summary>
        public static TheoryData<ILogger?, Exception?, string, string> NullParametersTestData =>
            new TheoryData<ILogger?, Exception?, string, string>
            {
                { null, new Exception(), "operation", "logger" },
                { Mock.Of<ILogger>(), null, "operation", "ex" }
            };

        [Theory]
        [MemberData(nameof(NullParametersTestData))]
        public void HandleException_NullParameters_ThrowsArgumentNull(
            ILogger? logger,
            Exception? ex,
            string operation,
            string expectedParamName)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                CacheExceptionHandler.HandleException<CacheException>(
                    logger!,
                    ex!,
                    operation));

            Assert.Equal(expectedParamName, exception.ParamName);
        }

        /// <summary>
        /// Verifica il comportamento con operation vuota
        /// </summary>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void HandleException_EmptyOperation_ThrowsArgumentException(string operation)
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                CacheExceptionHandler.HandleException<CacheException>(
                    loggerMock.Object,
                    new Exception(),
                    operation));

            Assert.Equal("operation", ex.ParamName);
        }

        /// <summary>
        /// Eccezione di test con costruttore non valido
        /// </summary>
        private class InvalidCacheException : CacheException
        {
            public InvalidCacheException() : base("Invalid", new Exception()) { }
        }
    }
}