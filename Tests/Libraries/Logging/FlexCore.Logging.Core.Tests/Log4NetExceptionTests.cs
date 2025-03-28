using Xunit;
using System;
using FlexCore.Logging.Core;
using FleFlexCore.Logging.Core.Exceptions;

namespace FlexCore.Logging.Core.Tests
{
    /// <summary>
    /// Test per la classe Log4NetException
    /// </summary>
    public class Log4NetExceptionTests
    {
        /// <summary>
        /// Verifica che Log4NetException possa essere istanziata con un messaggio.
        /// </summary>
        [Fact]
        public void Constructor_WithMessage_ShouldSetMessage()
        {
            var ex = new Log4NetException("Test exception");

            Assert.Equal("Test exception", ex.Message);
        }

        /// <summary>
        /// Verifica che Log4NetException possa essere istanziata con un messaggio e un'inner exception.
        /// </summary>
        [Fact]
        public void Constructor_WithMessageAndInnerException_ShouldSetProperties()
        {
            var innerEx = new Exception("Inner exception");
            var ex = new Log4NetException("Test exception", innerEx);

            Assert.Equal("Test exception", ex.Message);
            Assert.Equal(innerEx, ex.InnerException);
        }
    }
}
