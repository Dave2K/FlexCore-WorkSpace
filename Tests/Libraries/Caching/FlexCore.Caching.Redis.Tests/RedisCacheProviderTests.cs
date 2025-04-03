using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Moq;
using StackExchange.Redis;
using Xunit;
using Microsoft.Extensions.Logging;

namespace FlexCore.Caching.Redis.Tests
{
    /// <summary>
    /// Test unitari per la classe <see cref="RedisCacheProvider"/>.
    /// </summary>
    public class RedisCacheProviderTests : IDisposable
    {
        private readonly Mock<IConnectionMultiplexer> _connectionMock;
        private readonly Mock<IDatabase> _databaseMock;
        private readonly Mock<ILogger<RedisCacheProvider>> _loggerMock;
        private readonly RedisCacheProvider _provider;

        /// <summary>
        /// Inizializza i mock e l'istanza del provider da testare.
        /// </summary>
        public RedisCacheProviderTests()
        {
            _connectionMock = new Mock<IConnectionMultiplexer>();
            _databaseMock = new Mock<IDatabase>();
            _loggerMock = new Mock<ILogger<RedisCacheProvider>>();

            _connectionMock
                .Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(_databaseMock.Object);

            _provider = new RedisCacheProvider(
                "localhost:6379",
                _loggerMock.Object,
                _connectionMock.Object
            );
        }

        // ... [Inserire qui tutti i test visti in precedenza con commenti XML] ...

        /// <summary>
        /// Rilascia le risorse del provider dopo ogni test.
        /// </summary>
        public void Dispose()
        {
            _provider.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}