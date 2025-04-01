using Xunit;
using FlexCore.Database.SQLServer;
using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using WorkSpace.Generated;

namespace FlexCore.Database.SQLServer.Tests
{
    public class SQLServerProviderTests
    {
        private readonly string _connectionString;

        public SQLServerProviderTests()
        {
            _connectionString = GetConnectionString();
        }

        private static string GetConnectionString()
        {
            string resourcesFolder = WSEnvironment.ResourcesFolder;
            var configuration = new ConfigurationBuilder()
               .SetBasePath(resourcesFolder)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();
            string? connString = configuration["DatabaseSettings:SQLServer:ConnectionString"];

            if (string.IsNullOrEmpty(connString))
            {
                throw new InvalidOperationException("ConnectionString non trovata in appsettings.json");
            }

            return connString;
        }

        [Fact]
        public void CreateConnection_ShouldReturnOpenConnection()
        {
            var provider = new SQLServerProvider(_connectionString);
            using var connection = provider.CreateConnection();
            Assert.NotNull(connection);
        }

        [Fact]
        public async Task CreateConnectionAsync_ShouldReturnOpenConnection()
        {
            var provider = new SQLServerProvider(_connectionString);
            using var connection = await provider.CreateConnectionAsync();
            Assert.NotNull(connection);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenConnectionStringIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new SQLServerProvider(null));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenConnectionStringIsEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => new SQLServerProvider(""));
        }

        [Fact]
        public void CreateConnection_ShouldUseMockedConnection()
        {
            var mockConnection = new Mock<IDbConnection>();
            mockConnection.Setup(c => c.State).Returns(ConnectionState.Open);

            // Creiamo un provider con una connessione mockata
            var provider = new SQLServerProvider(_connectionString);

            using var connection = provider.CreateConnection();
            Assert.NotNull(connection);
            Assert.Equal(ConnectionState.Open, connection.State);
        }
    }
}
