using FlexCore.Caching.Redis;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace FlexCore.Caching.Redis.Tests
{
    /// <summary>
    /// Test suite per la classe <see cref="RedisCacheException"/>
    /// </summary>
    public class RedisCacheExceptionFullCoverageTests
    {
        /// <summary>
        /// Verifica che il costruttore completo registri l'errore correttamente
        /// </summary>
        [Fact]
        public void FullConstructor_ShouldSetPropertiesAndLogError()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<RedisCacheProvider>>();
            Exception realException;
            try
            {
                throw new Exception("Errore interno simulato");
            }
            catch (Exception ex)
            {
                realException = ex; // Genera stack trace reale
            }
            const string message = "Errore Redis critico";

            // Act
            var redisEx = new RedisCacheException(loggerMock.Object, message, realException); // Rinominata variabile

            // Assert
            Assert.Equal(message, redisEx.Message);
            Assert.Same(realException, redisEx.InnerException);

            // Verifica pattern esatto del log
            loggerMock.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) =>
                    v.ToString()!.Contains($"Errore Redis - Messaggio: {message}") &&
                    v.ToString()!.Contains($"Tipo: {realException.GetType().FullName}") &&
                    v.ToString()!.Contains($"Stack: {realException.StackTrace}")),
                realException,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        /// <summary>
        /// Verifica il lancio di eccezione per messaggio nullo
        /// </summary>
        [Fact]
        public void Constructor_NullMessage_ShouldThrowArgumentException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<RedisCacheProvider>>();
            var innerEx = new Exception("Test");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new RedisCacheException(loggerMock.Object, null!, innerEx));

            Assert.Equal("message", ex.ParamName);
            Assert.Contains("deve contenere informazioni significative", ex.Message);
        }

        /// <summary>
        /// Verifica il comportamento con inner exception nulla
        /// </summary>
        [Fact]
        public void Constructor_NullInnerException_ShouldNotThrow()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<RedisCacheProvider>>();
            const string message = "Errore generico";

            // Act
            var ex = new RedisCacheException(loggerMock.Object, message, null!);

            // Assert
            Assert.Equal(message, ex.Message);
            Assert.Null(ex.InnerException);
        }

        /// <summary>
        /// Verifica la creazione con costruttore semplificato
        /// </summary>
        [Theory]
        [InlineData("Errore connessione")]
        [InlineData("Errore serializzazione")]
        public void SimpleConstructor_ShouldCreateValidException(string message)
        {
            // Act
            var ex = new RedisCacheException(message);

            // Assert
            Assert.Equal(message, ex.Message);
            Assert.Null(ex.InnerException);
        }

        /// <summary>
        /// Verifica la generazione dello stack trace combinato
        /// </summary>
        [Fact]
        public void StackTrace_ShouldCombineInnerAndOuterStacks()
        {
            // Arrange
            Exception innerEx;
            try
            {
                throw new InvalidOperationException("Errore interno");
            }
            catch (Exception ex)
            {
                innerEx = ex;
            }
            var outerEx = new RedisCacheException("Errore esterno", innerEx);

            // Act
            var fullStackTrace = outerEx.ToString();

            // Assert
            Assert.NotNull(fullStackTrace);
            Assert.Contains("--- End of inner exception stack trace ---", fullStackTrace);
            Assert.Contains(innerEx.StackTrace!, fullStackTrace);
        }
    }
}