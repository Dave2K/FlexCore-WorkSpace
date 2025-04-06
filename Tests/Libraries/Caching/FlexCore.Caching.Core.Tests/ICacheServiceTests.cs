using FlexCore.Caching.Core.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FlexCore.Caching.Core.Tests.Interfaces
{
    /// <summary>
    /// Test suite per verificare il contratto completo dell'interfaccia <see cref="ICacheService"/>.
    /// </summary>
    public class ICacheServiceTests
    {
        /// <summary>
        /// Verifica che l'interfaccia implementi correttamente tutti i metodi del contratto.
        /// </summary>
        [Fact]
        public async Task ICacheService_ShouldImplementFullContractAsync()
        {
            // Arrange
            var mockService = new Mock<ICacheService>();

            // Configurazione dei metodi ASYNC (versione corretta)
            mockService.Setup(s => s.GetAsync<string>(It.IsAny<string>()))
                .ReturnsAsync("test_value");

            mockService.Setup(s => s.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()))
                .ReturnsAsync(true);

            mockService.Setup(s => s.RemoveAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            mockService.Setup(s => s.ExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var resultGet = await mockService.Object.GetAsync<string>("key");
            var resultSet = await mockService.Object.SetAsync("key", "value", TimeSpan.FromMinutes(1));
            var resultRemove = await mockService.Object.RemoveAsync("key");
            var resultExists = await mockService.Object.ExistsAsync("key");

            // Assert
            Assert.NotNull(mockService.Object);
            Assert.Equal("test_value", resultGet);
            Assert.True(resultSet);
            Assert.True(resultRemove);
            Assert.True(resultExists);
        }

        /// <summary>
        /// Verifica che l'interfaccia contenga solo metodi asincroni.
        /// </summary>
        [Fact]
        public void Interface_ShouldContainOnlyAsyncMethods()
        {
            // Arrange
            var methods = typeof(ICacheService).GetMethods();

            // Act
            var syncMethods = methods
                .Where(m => !m.Name.EndsWith("Async", StringComparison.Ordinal))
                .ToList();

            // Assert
            Assert.Empty(syncMethods);
        }
    }
}