using Xunit;
using System;
using FlexCore.Logging.Core;

namespace FlexCore.Logging.Core.Tests
{
    /// <summary>
    /// Test per la classe SerilogException
    /// </summary>
    public class SerilogExceptionTests
    {
        /// <summary>
        /// Verifica che SerilogException possa essere istanziata con un messaggio.
        /// </summary>
        [Fact]
        public void Constructor_WithMessage_ShouldSetMessage()
        {
            var ex = new SerilogException("Test exception");

            Assert.Equal("Test exception", ex.Message);
        }

        /// <summary>
        /// Verifica che SerilogException possa essere istanziata con un messaggio e un'inner exception.
        /// </summary>
        [Fact]
        public void Constructor_WithMessageAndInnerException_ShouldSetProperties()
        {
            var innerEx = new Exception("Inner exception");
            var ex = new SerilogException("Test exception", innerEx);

            Assert.Equal("Test exception", ex.Message);
            Assert.Equal(innerEx, ex.InnerException);
        }
    }
}
