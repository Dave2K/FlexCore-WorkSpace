// File: RedisCacheProviderSerializationTests.cs
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
    /// Test per la gestione degli errori di serializzazione/deserializzazione
    /// </summary>
    public class RedisCacheProviderSerializationTests : IDisposable
    {
        private readonly Mock<IConnectionMultiplexer> _mockConnection;
        private readonly RedisCacheProvider _provider;
        private readonly Mock<ILogger<RedisCacheProvider>> _mockLogger;
        private readonly IDatabase _mockDatabase;

        public RedisCacheProviderSerializationTests()
        {
            _mockConnection = new Mock<IConnectionMultiplexer>();
            _mockLogger = new Mock<ILogger<RedisCacheProvider>>();

            // Configurazione mock del database
            _mockDatabase = new Mock<IDatabase>().Object;
            _mockConnection.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(_mockDatabase);

            _provider = new RedisCacheProvider(
                "localhost:6379",
                _mockLogger.Object,
                _mockConnection.Object
            );
        }

        /// <summary>
        /// Verifica il comportamento con dati JSON non validi
        /// </summary>
        [Fact]
        public async Task GetAsync_InvalidJson_ShouldLogAndReturnDefault()
        {
            // Arrange
            const string corruptedKey = "corrupted_key";
            var mockDb = new Mock<IDatabase>();
            mockDb.Setup(db => db.StringGetAsync(corruptedKey, CommandFlags.None))
                .ReturnsAsync(RedisValue.Unbox("invalid_json"));

            _mockConnection.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(mockDb.Object);

            // Act & Assert
            await Assert.ThrowsAsync<JsonException>(() => _provider.GetAsync<object>(corruptedKey));

            _mockLogger.Verify(log => log.LogError(
                It.IsAny<JsonException>(),
                It.Is<string>(msg => msg.Contains("Errore durante la deserializzazione")),
                corruptedKey
            ), Times.Once);
        }

        public void Dispose() => _mockConnection.Reset();
    }
}