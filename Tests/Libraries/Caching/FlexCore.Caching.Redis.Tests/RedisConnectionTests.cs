using FlexCore.Caching.Redis;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;
using System;
using Xunit;

namespace FlexCore.Caching.Redis.Tests
{
    /// <summary>
    /// Test per la gestione delle connessioni Redis
    /// </summary>
    public class RedisConnectionTests : IDisposable
    {
        private readonly Mock<IConnectionMultiplexer> _connectionMock;
        private readonly RedisCacheProvider _provider;
        private readonly Mock<ILogger<RedisCacheProvider>> _loggerMock;

        /// <summary>
        /// Inizializza una nuova istanza della classe di test
        /// </summary>
        public RedisConnectionTests()
        {
            _connectionMock = new Mock<IConnectionMultiplexer>();
            _loggerMock = new Mock<ILogger<RedisCacheProvider>>();

            // Chiamata esplicita a tutti i parametri (nessun optional)
            _provider = new RedisCacheProvider(
                connectionString: "localhost:6379",
                logger: _loggerMock.Object,
                connection: _connectionMock.Object,
                serializerOptions: null // Parametro opzionale esplicitato
            );
        }

        /// <summary>
        /// Verifica il comportamento con stringhe di connessione non valide
        /// </summary>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("invalid_host")]
        public void Constructor_InvalidConnectionString_ShouldThrow(string invalidConnection)
        {
            // Arrange
            var loggerMock = new Mock<ILogger<RedisCacheProvider>>();

            // Act & Assert
            // Chiamata corretta con tutti i parametri obbligatori e opzionali
            var ex = Assert.Throws<ArgumentException>(() =>
                new RedisCacheProvider(
                    connectionString: invalidConnection,
                    logger: loggerMock.Object,
                    connection: null, // Parametro opzionale esplicitato
                    serializerOptions: null // Parametro opzionale esplicitato
                ));

            Assert.Contains("connectionString", ex.Message);
        }

        /// <summary>
        /// Smaltimento delle risorse
        /// </summary>
        public void Dispose() => _provider.Dispose();
    }
}