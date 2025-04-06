using FlexCore.Caching.Core.Interfaces;
using Moq;
using Xunit;

namespace FlexCore.Caching.Core.Tests.Interfaces
{
    /// <summary>
    /// Test suite per l'interfaccia <see cref="ICacheService"/>
    /// </summary>
    public class ICacheServiceTests
    {
        /// <summary>
        /// Verifica il contratto completo dell'interfaccia
        /// </summary>
        [Fact]
        public void ICacheService_ShouldImplementFullContract()
        {
            // Arrange
            var mockService = new Mock<ICacheService>();

            // Act & Assert
            mockService.Setup(s => s.Get<string>(It.IsAny<string>()));
            mockService.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()));
            mockService.Setup(s => s.Remove(It.IsAny<string>()));
            mockService.Setup(s => s.Exists(It.IsAny<string>()));

            Assert.NotNull(mockService.Object);
        }
    }
}       