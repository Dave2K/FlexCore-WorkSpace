using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using FlexCore.Caching.Common.Exceptions;

namespace FlexCore.Caching.Common.Tests
{
    public class CacheExceptionHandlerTests
    {
        [Fact]
        public void HandleException_ValidOperation_LogsError()
        {
            var loggerMock = new Mock<ILogger>();
            var innerEx = new Exception("Test inner");
            var cacheEx = new CacheException("Test message", innerEx); // Usa CacheException

            CacheExceptionHandler.HandleException<CacheException>(
                loggerMock.Object,
                cacheEx, // Passa CacheException invece di Exception
                "SET",
                "testKey",
                "TestContext"
            );

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    cacheEx,
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>() // Aggiungi '?'
                ),
                Times.Once
            );
        }
    }
}