using Xunit;
using Moq;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using System.Net;

namespace FlexCore.Caching.Redis.Tests
{
    /// <summary>
    /// Test suite per RedisCacheProvider
    /// </summary>
    public class RedisCacheProviderTests : IDisposable
    {
        private readonly Mock<IConnectionMultiplexer> _connectionMock;
        private readonly Mock<IDatabase> _databaseMock;
        private readonly RedisCacheProvider _provider;
        private bool _disposed;

        /// <summary>
        /// Opzioni di serializzazione condivise
        /// </summary>
        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// Inizializza una nuova istanza della classe di test
        /// </summary>
        public RedisCacheProviderTests()
        {
            _connectionMock = new Mock<IConnectionMultiplexer>();
            _databaseMock = new Mock<IDatabase>();
            var loggerMock = new Mock<ILogger<RedisCacheProvider>>();

            // Configurazione mock per Redis
            _connectionMock.Setup(c => c.GetEndPoints(It.IsAny<bool>()))
                         .Returns(new[] { new DnsEndPoint("localhost", 6379) });

            _connectionMock.Setup(c => c.GetDatabase(
                It.IsAny<int>(),
                It.IsAny<object>()))
                .Returns(_databaseMock.Object);

            _provider = new RedisCacheProvider(
                "localhost:6379",
                loggerMock.Object,
                _connectionMock.Object,
                _serializerOptions
            );
        }

        /// <summary>
        /// Verifica il corretto funzionamento di ExistsAsync
        /// </summary>
        [Fact]
        public async Task ExistsAsync_KeyExists_ReturnsTrue()
        {
            // Arrange
            _databaseMock.Setup(db => db.KeyExistsAsync("key", CommandFlags.None))
                        .ReturnsAsync(true);

            // Act
            var result = await _provider.ExistsAsync("key");

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Verifica la deserializzazione corretta di GetAsync
        /// </summary>
        [Fact]
        public async Task GetAsync_ValidKey_ReturnsDeserializedObject()
        {
            // Arrange
            var testObj = new { Id = 1, Name = "Test" };
            var serialized = JsonSerializer.Serialize(testObj, _serializerOptions);

            _databaseMock.Setup(db => db.StringGetAsync("key", CommandFlags.None))
                        .ReturnsAsync(serialized);

            // Act
            var result = await _provider.GetAsync<TestClass>("key");

            // Assert
            result.Should().BeEquivalentTo(testObj);
        }

        /// <summary>
        /// Verifica il comportamento di RemoveAsync
        /// </summary>
        [Fact]
        public async Task RemoveAsync_ValidKey_DeletesKey()
        {
            // Arrange
            _databaseMock.Setup(db => db.KeyDeleteAsync("key", CommandFlags.None))
                        .ReturnsAsync(true);

            // Act
            var result = await _provider.RemoveAsync("key");

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Verifica il corretto svuotamento della cache
        /// </summary>
        [Fact]
        public async Task ClearAllAsync_FlushesAllDatabases()
        {
            // Arrange
            var serverMock = new Mock<IServer>();
            serverMock.Setup(s => s.FlushAllDatabasesAsync(CommandFlags.None))
                     .Returns(Task.CompletedTask);

            _connectionMock.Setup(c => c.GetServer(It.IsAny<EndPoint>(), It.IsAny<object>()))
                          .Returns(serverMock.Object);

            // Act
            await _provider.ClearAllAsync();

            // Assert
            serverMock.Verify(s => s.FlushAllDatabasesAsync(CommandFlags.None), Times.Once);
        }

        /// <summary>
        /// Rilascia le risorse del test
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _provider.Dispose();
            _disposed = true;
        }

        /// <summary>
        /// Classe di test per la deserializzazione
        /// </summary>
        private class TestClass
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }
    }
}