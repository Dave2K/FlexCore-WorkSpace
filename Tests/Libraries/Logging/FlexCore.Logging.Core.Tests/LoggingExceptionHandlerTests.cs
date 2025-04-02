using FlexCore.Logging.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace FlexCore.Logging.Core.Tests
{
    public class LoggingExceptionHandlerTests
    {
        [Fact]
        public void HandleLoggingException_LogsErrorAndReturnsException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var ex = new Exception("Test error");

            // Act
            var result = LoggingExceptionHandler.HandleLoggingException<InvalidOperationException>(
                loggerMock.Object,
                ex,
                "TestComponent",
                "TestOperation"
            );

            // Assert
            loggerMock.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                ex,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>() // ✅ Corretto qui
            ));
            Assert.IsType<InvalidOperationException>(result);
            Assert.Contains("TestComponent", result.Message);
        }
    }
}