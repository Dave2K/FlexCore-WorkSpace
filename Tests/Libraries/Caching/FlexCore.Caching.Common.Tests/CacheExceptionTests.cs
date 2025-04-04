using Xunit;
using FlexCore.Caching.Common.Exceptions;
using System;

namespace FlexCore.Caching.Common.Tests
{
    /// <summary>
    /// Test suite per la classe <see cref="CacheException"/>
    /// </summary>
    public class CacheExceptionTests
    {
        /// <summary>
        /// Verifica che l'eccezione contenga correttamente il messaggio e l'inner exception
        /// </summary>
        [Fact]
        public void Constructor_WithMessageAndInnerException_ShouldSetProperties()
        {
            // Arrange
            const string expectedMessage = "Test message";
            var innerException = new InvalidOperationException();

            // Act
            var exception = new CacheException(expectedMessage, innerException);

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
            Assert.Same(innerException, exception.InnerException);
        }

        /// <summary>
        /// Verifica il comportamento con messaggio nullo
        /// </summary>
        [Fact]
        public void Constructor_WithNullMessage_ShouldHandleGracefully()
        {
            // Arrange
            string? nullMessage = null;
            var innerException = new Exception();

            // Act
            var exception = new CacheException(nullMessage!, innerException);

            // Assert
            Assert.Contains("Exception of type", exception.Message);
        }
    }
}