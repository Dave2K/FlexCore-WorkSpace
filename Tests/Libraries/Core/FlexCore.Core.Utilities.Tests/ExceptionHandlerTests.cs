using FlexCore.Core.Utilities;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace FlexCore.Core.Utilities.Tests
{
    /// <summary>
    /// Test per la classe ExceptionHandler
    /// </summary>
    public class ExceptionHandlerTests
    {
        private class TestException : Exception
        {
            /// <summary>
            /// Costruttore per test
            /// </summary>
            public TestException(string message, Exception inner)
                : base(message, inner) { }
        }

        private class InvalidTestException : Exception
        {
            /// <summary>
            /// Costruttore non valido per test
            /// </summary>
            public InvalidTestException() { }
        }

        /// <summary>
        /// Verifica la corretta creazione di un'eccezione tipizzata
        /// </summary>
        [Fact]
        public void HandleException_CreatesTypedException()
        {
            var loggerMock = new Mock<ILogger>();
            var ex = new Exception("Test interno");

            var result = ExceptionHandler.HandleException<TestException>(
                loggerMock.Object,
                ex,
                "Test operation");

            Assert.IsType<TestException>(result);
            Assert.Equal("Errore durante Test operation", result.Message);
            Assert.Same(ex, result.InnerException);
        }

        /// <summary>
        /// Verifica il lancio di eccezione per costruttori non validi
        /// </summary>
        [Fact]
        public void HandleException_ThrowsWhenWrongConstructor()
        {
            var loggerMock = new Mock<ILogger>();
            var ex = new Exception("Test interno");

            var actualEx = Assert.Throws<InvalidOperationException>(() =>
                ExceptionHandler.HandleException<InvalidTestException>(
                    loggerMock.Object,
                    ex,
                    "Test operation"));

            Assert.Contains("(string, Exception)", actualEx.Message);
        }
    }
}