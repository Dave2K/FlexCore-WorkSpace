using Xunit;
using Moq;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using FlexCore.Caching.Redis;
using System.Net;

namespace FlexCore.Caching.Redis.Tests
{
    /// <summary>
    /// Test per la classe RedisCacheProvider
    /// </summary>
    public class RedisCacheProviderTests
    {
        private readonly Mock<IConnectionMultiplexer> _connectionMock = new();
        private readonly Mock<IDatabase> _databaseMock = new();
        private readonly Mock<ILogger<RedisCacheProvider>> _loggerMock = new();
        private readonly RedisCacheProvider _provider;

        public RedisCacheProviderTests()
        {
            _connectionMock.Setup(c => c.GetDatabase(
                It.IsAny<int>(),
                It.IsAny<object>()
            )).Returns(_databaseMock.Object);

            _provider = new RedisCacheProvider(
                _connectionMock.Object,
                _loggerMock.Object
            );
        }

        /// <summary>
        /// Verifica che il metodo ClearAll elimini tutte le chiavi
        /// </summary>
        [Fact]
        public void ClearAll_DeletesAllKeys()
        {
            // Arrange
            var serverMock = new Mock<IServer>();
            var keys = new RedisKey[] { "key1", "key2" };

            _connectionMock.Setup(c => c.GetEndPoints(It.IsAny<bool>()))
                .Returns([new DnsEndPoint("localhost", 6379)]);

            _connectionMock.Setup(c => c.GetServer(
                It.IsAny<System.Net.EndPoint>(),
                It.IsAny<object>() // Fix CS0854
            )).Returns(serverMock.Object);

            serverMock.Setup(s => s.Keys(
                It.IsAny<int>(),
                It.IsAny<RedisValue>(),
                It.IsAny<int>(),
                It.IsAny<long>(),
                It.IsAny<int>(),
                CommandFlags.None // Fix CS0854
            )).Returns(keys);

            // Act
            _provider.ClearAll();

            // Assert
            _databaseMock.Verify(
                x => x.KeyDelete(It.IsAny<RedisKey>(), CommandFlags.None),
                Times.Exactly(keys.Length)
            );
        }
    }
}