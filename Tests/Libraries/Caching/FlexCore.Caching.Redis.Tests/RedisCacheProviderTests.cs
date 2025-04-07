using FlexCore.Caching.Core.Interfaces;
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
    /// Test suite per verificare il comportamento di <see cref="RedisCacheProvider"/>.
    /// Include test per operazioni base, gestione errori e conformità ai contratti async.
    /// </summary>
    public class RedisCacheProviderTests : IDisposable
    {
        private readonly Mock<IConnectionMultiplexer> _connectionMock;
        private readonly Mock<IDatabase> _databaseMock;
        private readonly Mock<ILogger<RedisCacheProvider>> _loggerMock;
        private readonly RedisCacheProvider _provider;

        /// <summary>
        /// Inizializza una nuova istanza della classe di test.
        /// </summary>
        public RedisCacheProviderTests()
        {
            _connectionMock = new Mock<IConnectionMultiplexer>();
            _databaseMock = new Mock<IDatabase>();
            _loggerMock = new Mock<ILogger<RedisCacheProvider>>();

            _connectionMock.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                          .Returns(_databaseMock.Object);

            _provider = new RedisCacheProvider(
                "localhost:6379",
                _loggerMock.Object,
                _connectionMock.Object
            );
        }

        /// <summary>
        /// Verifica che GetAsync ritorni il valore salvato correttamente.
        /// </summary>
        [Fact]
        public async Task GetAsync_ValidKey_ReturnsStoredValue()
        {
            // Arrange
            const string key = "test_key";
            const string value = "test_value";
            _databaseMock.Setup(db => db.StringGetAsync(key, CommandFlags.None))
                        .ReturnsAsync(value);

            // Act
            var result = await _provider.GetAsync<string>(key);

            // Assert
            Assert.Equal(value, result);
        }

        /// <summary>
        /// Verifica che GetAsync lanci eccezione per chiave non valida.
        /// </summary>
        [Theory]
        [InlineData(null!)] // Test esplicito per null con null-forgiving operator
        [InlineData("")]
        [InlineData("  ")]
        public async Task GetAsync_InvalidKey_ThrowsArgumentException(string? invalidKey)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _provider.GetAsync<string>(invalidKey!) // ✅ Null-forgiving operator
            );
        }

        /// <summary>
        /// Verifica che SetAsync salvi correttamente un valore.
        /// </summary>
        [Fact]
        public async Task SetAsync_ValidData_ReturnsTrue()
        {
            // Arrange
            const string key = "test_key";
            _databaseMock.Setup(db => db.StringSetAsync(key, It.IsAny<RedisValue>(), It.IsAny<TimeSpan>(), false, When.Always, CommandFlags.None))
                        .ReturnsAsync(true);

            // Act
            var result = await _provider.SetAsync(key, "value", TimeSpan.FromMinutes(1));

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Verifica che ClearAllAsync svuoti tutte le chiavi.
        /// </summary>
        [Fact]
        public async Task ClearAllAsync_FlushesAllDatabases()
        {
            // Arrange
            var serverMock = new Mock<IServer>();
            _connectionMock.Setup(c => c.GetEndPoints(false))
                          .Returns(new[] { new Mock<EndPoint>().Object });
            _connectionMock.Setup(c => c.GetServer(It.IsAny<EndPoint>(), null))
                          .Returns(serverMock.Object);

            // Act
            await _provider.ClearAllAsync();

            // Assert
            serverMock.Verify(s => s.FlushAllDatabasesAsync(CommandFlags.None), Times.Once);
        }

        /// <summary>
        /// Verifica che gli errori di deserializzazione vengano loggati.
        /// </summary>
        [Fact]
        public async Task GetAsync_InvalidJson_LogsDeserializationError()
        {
            // Arrange
            const string key = "invalid_json_key";
            _databaseMock.Setup(db => db.StringGetAsync(key, CommandFlags.None))
                        .ReturnsAsync("{invalid_json}");

            // Act
            await _provider.GetAsync<object>(key);

            // Assert
            _loggerMock.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Deserializzazione fallita")), // ✅ Null-forgiving
                It.IsAny<JsonException>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>() // ✅ Exception nullable
            ));
        }

        /// <summary>
        /// Libera le risorse allocate per i test.
        /// </summary>
        public void Dispose()
        {
            _provider.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}