// File: RedisCacheProviderTests.cs
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace FlexCore.Caching.Redis.Tests
{
    /// <summary>
    /// Test suite per RedisCacheProvider
    /// </summary>
    public class RedisCacheProviderTests : IDisposable
    {
        private readonly Mock<IConnectionMultiplexer> _mockConnection;
        private readonly Mock<ILogger<RedisCacheProvider>> _mockLogger;
        private readonly RedisCacheProvider _provider;
        private readonly Mock<IDatabase> _mockDatabase;

        /// <summary>
        /// Inizializza una nuova istanza dei test
        /// </summary>
        public RedisCacheProviderTests()
        {
            _mockConnection = new Mock<IConnectionMultiplexer>();
            _mockLogger = new Mock<ILogger<RedisCacheProvider>>();
            _mockDatabase = new Mock<IDatabase>();

            _mockConnection.Setup(c => c.GetDatabase(
                    It.IsAny<int>(),
                    It.IsAny<object>()))
                .Returns(_mockDatabase.Object);

            _provider = new RedisCacheProvider(
                "localhost:6379",
                _mockLogger.Object,
                _mockConnection.Object);
        }

        /// <summary>
        /// Verifica il corretto smaltimento delle risorse
        /// </summary>
        public void Dispose()
        {
            _provider.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Test per il comportamento di ClearAllAsync in caso di errore di connessione
        /// </summary>
        [Fact]
        public async Task ClearAllAsync_ThrowsRedisConnectionException_WhenConnectionFails()
        {
            // Arrange
            var mockServer = new Mock<IServer>();
            _mockConnection.Setup(c => c.GetServer(
                    It.IsAny<EndPoint>(),
                    It.IsAny<CommandFlags>()))
                .Returns(mockServer.Object);

            var exception = new RedisConnectionException(
                ConnectionFailureType.UnableToConnect,
                "Errore di connessione");

            mockServer.Setup(s => s.FlushAllDatabasesAsync(
                    It.IsAny<CommandFlags>()))
                .ThrowsAsync(exception);

            // Act & Assert
            await Assert.ThrowsAsync<RedisConnectionException>(
                () => _provider.ClearAllAsync());
        }

        /// <summary>
        /// Test per GetAsync con deserializzazione fallita
        /// </summary>
        [Fact]
        public async Task GetAsync_InvalidData_ShouldThrowJsonException()
        {
            // Arrange
            const string key = "invalid_key";
            _mockDatabase.Setup(db => db.StringGetAsync(
                    key,
                    It.IsAny<CommandFlags>()))
                .ReturnsAsync(RedisValue.Unbox("{invalid_json}"));

            // Act & Assert
            await Assert.ThrowsAsync<JsonException>(
                () => _provider.GetAsync<object>(key));
        }

        /// <summary>
        /// Test per SetAsync con dati validi
        /// </summary>
        [Fact]
        public async Task SetAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            const string key = "valid_key";
            _mockDatabase.Setup(db => db.StringSetAsync(
                    key,
                    It.IsAny<RedisValue>(),
                    It.IsAny<TimeSpan>(),
                    It.IsAny<When>(),
                    It.IsAny<CommandFlags>()))
                .ReturnsAsync(true);

            // Act
            var result = await _provider.SetAsync(key, "value", TimeSpan.FromMinutes(1));

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test per RemoveAsync con chiave esistente
        /// </summary>
        [Fact]
        public async Task RemoveAsync_ExistingKey_ShouldReturnTrue()
        {
            // Arrange
            const string key = "existing_key";
            _mockDatabase.Setup(db => db.KeyDeleteAsync(
                    key,
                    It.IsAny<CommandFlags>()))
                .ReturnsAsync(true);

            // Act
            var result = await _provider.RemoveAsync(key);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test per ExistsAsync con chiave inesistente
        /// </summary>
        [Fact]
        public async Task ExistsAsync_NonExistingKey_ShouldReturnFalse()
        {
            // Arrange
            const string key = "non_existing_key";
            _mockDatabase.Setup(db => db.KeyExistsAsync(
                    key,
                    It.IsAny<CommandFlags>()))
                .ReturnsAsync(false);

            // Act
            var result = await _provider.ExistsAsync(key);

            // Assert
            Assert.False(result);
        }
    }
}