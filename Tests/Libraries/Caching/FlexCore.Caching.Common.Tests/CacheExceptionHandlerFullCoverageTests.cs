using FlexCore.Caching.Common.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace FlexCore.Caching.Common.Tests
{
    /// <summary>
    /// Test per garantire la copertura completa della classe <see cref="CacheExceptionHandler"/>
    /// </summary>
    public class CacheExceptionHandlerFullCoverageTests
    {
        /// <summary>
        /// Verifica la creazione corretta di un'eccezione tipizzata con contesto completo
        /// </summary>
        [Fact]
        public void HandleException_ValidParameters_ShouldReturnTypedException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var innerEx = new InvalidOperationException("Operazione non valida");
            const string operation = "Salvataggio cache";
            const string key = "item_123";

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
            loggerMock.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains(operation)),
                innerEx,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()));
        }

        /// <summary>
        /// Verifica il lancio di eccezione per tipo senza costruttore corretto
        /// </summary>
        [Fact]
        public void HandleException_MissingConstructor_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var innerEx = new Exception("Errore generico");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                CacheExceptionHandler.HandleException<InvalidCacheException>(
                    loggerMock.Object,
                    innerEx,
                    "Test operation"));
        }

        /// <summary>
        /// Verifica il comportamento con parametri nulli
        /// </summary>
        /// <param name="operation">Nome dell'operazione (null per testare il logger nullo)</param>
        [Theory]
        [InlineData(null)] // Test per logger null
        [InlineData("invalid_operation")] // Test per operazione non valida
        public void HandleException_NullParameters_ShouldThrow(string? operation)
        {
            // Arrange
            var logger = operation == null ? null! : new Mock<ILogger>().Object;
            var validOperation = operation ?? "default_operation";
            var innerEx = new Exception("Errore interno"); // ✅ Valore sempre valido

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                CacheExceptionHandler.HandleException<CacheException>(
                    logger,
                    innerEx,
                    validOperation));

            Assert.Equal("logger", ex.ParamName); // Verifica il parametro nullo corretto
        }

        /// <summary>
        /// Eccezione di test senza costruttore valido
        /// </summary>
        private class InvalidCacheException : CacheException
        {
            public InvalidCacheException() : base("Costruttore non valido") { }
        }
    }
}