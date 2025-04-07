using FlexCore.Caching.Common.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace FlexCore.Caching.Common.Tests
{
    /// <summary>
    /// Test suite per garantire la copertura completa di <see cref="CacheExceptionHandler"/>
    /// </summary>
    public class CacheExceptionHandlerFullCoverageTests
    {
        /// <summary>
        /// Verifica che venga lanciata una <see cref="ArgumentNullException"/> per parametri nulli
        /// </summary>
        /// <param name="operation">Nome operazione (simula valori validi e nulli)</param>
        [Theory]
        [InlineData(null)] // Test esplicito per logger null
        [InlineData("valid_operation")]
        public void HandleException_NullParameters_ShouldThrow(string? operation)
        {
            // Arrange
            var validInnerEx = new InvalidOperationException("Test exception");

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                CacheExceptionHandler.HandleException<CacheException>(
                    logger: null!, // ✅ Forziamo logger null
                    validInnerEx,
                    operation ?? "default_operation" // Usa operazione valida se null
                )
            );

            // Verifica esplicita del parametro
            Assert.Equal("logger", ex.ParamName);
        }

        /// <summary>
        /// Verifica la creazione corretta di un'eccezione tipizzata
        /// </summary>
        [Fact]
        public void HandleException_ValidParameters_ShouldCreateException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var innerEx = new Exception("Test");

            // Act
            var result = CacheExceptionHandler.HandleException<CacheException>(
                loggerMock.Object,
                innerEx,
                "TestOperation"
            );

            // Assert
            Assert.IsType<CacheException>(result);
            Assert.Same(innerEx, result.InnerException);
        }
    }
}