using FlexCore.Caching.Common.Exceptions;
using FlexCore.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace FlexCore.Caching.Memory.Tests
{
    /// <summary>
    /// Test per garantire la copertura completa della classe MemoryCacheException.
    /// </summary>
    public class MemoryCacheExceptionFullCoverageTests
    {
        /// <summary>
        /// Verifica che il costruttore con logger lanci ArgumentException per messaggio vuoto.
        /// </summary>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_EmptyMessage_ShouldThrowArgumentException(string invalidMessage)
        {
            // Arrange
            var loggerMock = new Mock<ILogger<MemoryCacheException>>();
            var innerEx = new Exception("Test");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new MemoryCacheException(loggerMock.Object, invalidMessage, innerEx)
            );

            Assert.Equal("message", ex.ParamName); // ✅
        }
    }
}