using FlexCore.Caching.Redis;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace FlexCore.Caching.Redis.Tests
{
    /// <summary>
    /// Test per garantire la copertura completa della classe RedisCacheException
    /// </summary>
    public class RedisCacheExceptionFullCoverageTests
    {
        /// <summary>
        /// Verifica il costruttore con tutti i parametri
        /// </summary>
        [Fact]
        public void FullConstructor_ShouldSetPropertiesAndLogError()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<RedisCacheException>>();
            var innerEx = new Exception("Database connection failed");
            const string message = "Critical Redis error";

            // Act
            var ex = new RedisCacheException(loggerMock.Object, message, innerEx);

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
        /// Verifica il costruttore senza logger (branch non testato)
        /// </summary>
        [Fact]
        public void MinimalConstructor_ShouldSetPropertiesWithoutLogging()
        {
            // Arrange
            var innerEx = new TimeoutException("Operation timed out");
            const string message = "Timeout error";

            // Act
            var ex = new RedisCacheException(message, innerEx);

            // Assert
            Assert.Equal(message, ex.Message);
            Assert.Same(innerEx, ex.InnerException);
        }

        /// <summary>
        /// Verifica il comportamento con messaggio nullo (branch non testato)
        /// </summary>
        [Fact]
        public void Constructor_NullMessage_ShouldThrowArgumentException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<RedisCacheException>>();
            var innerEx = new Exception("Test");

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new RedisCacheException(loggerMock.Object, null!, innerEx));
        }
    }
}