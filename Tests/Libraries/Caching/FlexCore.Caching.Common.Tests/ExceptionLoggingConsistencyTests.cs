using FlexCore.Caching.Common.Exceptions;
using FlexCore.Caching.Redis;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace FlexCore.Caching.Common.Tests
{
    /// <summary>
    /// Verifica che la gestione degli errori sia centralizzata e non duplicata
    /// </summary>
    public class ExceptionLoggingConsistencyTests
    {
        /// <summary>
        /// Verifica che un'eccezione venga loggata esattamente una volta
        /// </summary>
        [Fact]
        public void HandleException_ShouldLogOncePerError()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var innerEx = new InvalidOperationException("Test error");

            // Act
            var ex = CacheExceptionHandler.HandleException<CacheException>(
                loggerMock.Object,
                innerEx,
                "Test operation"
            );

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    innerEx,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }

        /// <summary>
        /// Verifica che le eccezioni concrete non aggiungano logging duplicato
        /// </summary>
        [Theory]
        [InlineData(typeof(MemoryCacheException))]
        [InlineData(typeof(RedisCacheException))]
        public void ConcreteExceptions_ShouldNotLogDirectly(Type exceptionType)
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var constructor = exceptionType.GetConstructor(new[]
            {
                typeof(ILogger<>).MakeGenericType(exceptionType),
                typeof(string),
                typeof(Exception)
            });

            // Act/Assert
            if (constructor != null)
            {
                var ex = (Exception)constructor.Invoke(new object[]
                {
                    loggerMock.Object,
                    "Test message",
                    new Exception()
                });

                loggerMock.Verify(
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
        }
    }
}