using FlexCore.Caching.Common.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FlexCore.Caching.Tests.Exceptions
{
    /// <summary>
    /// Test suite per la classe <see cref="CacheExceptionHandler"/>
    /// </summary>
    public class CacheExceptionHandlerTests
    {
        private readonly Mock<ILogger> _mockLogger = new();

        /// <summary>
        /// Verifica che venga lanciata una <see cref="ArgumentNullException"/> quando il logger è null
        /// </summary>
        [Fact]
        public void HandleException_ThrowsArgumentNullException_WhenLoggerIsNull()
        {
            var ex = new Exception("Test exception");

            var exception = Assert.Throws<ArgumentNullException>(
                () => CacheExceptionHandler.HandleException<MemoryCacheException>(
                    null!, ex, "Read operation"));

            Assert.Equal("logger", exception.ParamName);
        }

        /// <summary>
        /// Verifica che venga lanciata una <see cref="ArgumentNullException"/> quando l'eccezione originale è null
        /// </summary>
        [Fact]
        public void HandleException_ThrowsArgumentNullException_WhenExceptionIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => CacheExceptionHandler.HandleException<MemoryCacheException>(
                    _mockLogger.Object, null!, "Read operation"));
        }

        /// <summary>
        /// Verifica che venga lanciata una <see cref="ArgumentException"/> quando l'operazione è vuota
        /// </summary>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void HandleException_ThrowsArgumentException_WhenOperationIsEmpty(string invalidOperation)
        {
            var ex = new Exception("Test exception");

            var exception = Assert.Throws<ArgumentException>(
                () => CacheExceptionHandler.HandleException<MemoryCacheException>(
                    _mockLogger.Object, ex, invalidOperation));

            Assert.Equal("operation", exception.ParamName);
        }

        /// <summary>
        /// Verifica la corretta creazione di un'eccezione derivata con contesto completo
        /// </summary>
        [Fact]
        public void HandleException_CreatesCorrectExceptionType_WithFullContext()
        {
            var ex = new InvalidOperationException("Original exception");
            const string operation = "Caching operation";
            const string key = "test-key";
            const string caller = "TestMethod";

            var result = CacheExceptionHandler.HandleException<TestCacheException>(
                _mockLogger.Object, ex, operation, key, caller);

            Assert.IsType<TestCacheException>(result);
            Assert.Contains(operation, result.Message);
            Assert.Contains(key, result.Message);
            Assert.Contains(caller, result.Message);
            Assert.Same(ex, result.InnerException);
        }

        /// <summary>
        /// Verifica il logging corretto dell'errore
        /// </summary>
        [Fact]
        public void HandleException_LogsErrorWithCorrectMessage()
        {
            var ex = new Exception("Test exception");
            const string operation = "Write operation";
            const string expectedMessage = "[TestCaller] Operazione fallita: Write operation";

            CacheExceptionHandler.HandleException<MemoryCacheException>(
                _mockLogger.Object, ex, operation, caller: "TestCaller");

            _mockLogger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains(expectedMessage)),
                ex,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        /// <summary>
        /// Verifica che venga lanciata una <see cref="InvalidOperationException"/> per costruttore mancante
        /// </summary>
        [Fact]
        public void HandleException_ThrowsInvalidOperation_WhenWrongConstructor()
        {
            var ex = new Exception("Test exception");

            Assert.Throws<InvalidOperationException>(
                () => CacheExceptionHandler.HandleException<InvalidCacheException>(
                    _mockLogger.Object, ex, "Read operation"));
        }

        // Classi di test ausiliarie
        private class TestCacheException : CacheException
        {
            public TestCacheException(string message, Exception inner)
                : base(message, inner) { }
        }

        private class InvalidCacheException : CacheException
        {
            public InvalidCacheException() : base("Invalid") { } // Costruttore sbagliato
        }
    }
}