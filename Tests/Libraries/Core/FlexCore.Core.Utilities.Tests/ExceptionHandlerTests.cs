using FlexCore.Core.Utilities;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace FlexCore.Core.Utilities.Tests
{
    public class ExceptionHandlerTests
    {
        [Fact]
        public void HandleException_CreatesTypedException()
        {
            // Arrange
            var logger = new Mock<ILogger>().Object;
            var originalEx = new Exception("Test error");

            // Act & Assert
            void ExecuteTest() =>
                ExceptionHandler.HandleException<InvalidOperationException>( // ✅ Specifica esplicita del tipo
                    logger,
                    originalEx,
                    "TestOperation"
                );

            var ex = Assert.Throws<InvalidOperationException>(ExecuteTest);
            Assert.Contains("TestOperation", ex.Message);
        }
    }
}