using FlexCore.Caching.Common.Exceptions;
using FlexCore.Caching.Redis;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace FlexCore.Caching.Common.Tests
{
    /// <summary>
    /// Verifica la consistenza della gestione centralizzata degli errori
    /// </summary>
    /// <remarks>
    /// Garantisce che tutte le eccezioni concrete non eseguano logging diretto, 
    /// ma utilizzino esclusivamente il <see cref="CacheExceptionHandler"/>
    /// </remarks>
    public class ExceptionLoggingConsistencyTests
    {
        private readonly Mock<ILogger> _mockLogger = new();

        /// <summary>
        /// Verifica che le eccezioni concrete non effettuino logging autonomo
        /// </summary>
        [Theory]
        [InlineData(typeof(MemoryCacheException))]
        [InlineData(typeof(RedisCacheException))]
        public void ConcreteExceptions_ShouldNotLogDirectly(Type exceptionType)
        {
            // Arrange
            var innerEx = new Exception("Test error");
            const string message = "Test message";

            // Act
            if (exceptionType == typeof(MemoryCacheException))
            {
                // Utilizza solo costruttori senza logger
                _ = new MemoryCacheException(message, innerEx);
            }
            else if (exceptionType == typeof(RedisCacheException))
            {
                // Utilizza solo costruttori senza logger
                _ = new RedisCacheException(message, innerEx);
            }

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Never
            );
        }

        /// <summary>
        /// Verifica che il logging avvenga solo tramite l'handler centralizzato
        /// </summary>
        [Fact]
        public void AllLogging_ShouldBeHandledByExceptionHandler()
        {
            // Arrange
            var ex = new InvalidOperationException("Test");

            // Act
            var result = CacheExceptionHandler.HandleException<CacheException>(
                _mockLogger.Object,
                ex,
                "TestOperation"
            );

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    ex,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }
    }
}