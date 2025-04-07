using FlexCore.Caching.Redis;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace FlexCore.Caching.Redis.Tests
{
    /// <summary>
    /// Test suite per la gestione degli errori di serializzazione
    /// </summary>
    public class RedisCacheProviderSerializationTests : IDisposable
    {
        private readonly Mock<IConnectionMultiplexer> _mockConnection;
        private readonly Mock<IDatabase> _mockDatabase;
        private readonly Mock<ILogger<RedisCacheProvider>> _mockLogger;
        private readonly RedisCacheProvider _provider;

        public RedisCacheProviderSerializationTests()
        {
            _mockConnection = new Mock<IConnectionMultiplexer>();
            _mockLogger = new Mock<ILogger<RedisCacheProvider>>();
            _mockDatabase = new Mock<IDatabase>();

            _mockConnection.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                          .Returns(_mockDatabase.Object);

            _provider = new RedisCacheProvider(
                "localhost:6379",
                _mockLogger.Object,
                _mockConnection.Object
            );
        }

        /// <summary>
        /// Verifica che JSON malformato generi log corretti e ritorni default
        /// </summary>
        [Fact]
        public async Task GetAsync_InvalidJson_ShouldLogAndReturnDefault()
        {
            // Arrange
            const string key = "invalid_key";
            const string invalidJson = "{invalid_property: 123}"; // JSON non quotato
            _mockDatabase.Setup(db => db.StringGetAsync(key, CommandFlags.None))
                        .ReturnsAsync(invalidJson);

            // Act
            var result = await _provider.GetAsync<object>(key);

            // Assert
            Assert.Null(result);

            // ✅ Verifica esplicita del messaggio e del livello di log
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) =>
                        v.ToString().Contains("Deserializzazione fallita") &&
                        v.ToString().Contains(key)
                    ),
                    It.IsAny<JsonException>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }

        public void Dispose() => _provider.Dispose();
    }
}