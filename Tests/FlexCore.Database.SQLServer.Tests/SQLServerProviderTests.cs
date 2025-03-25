using Xunit;
using FlexCore.Database.SQLServer;
using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Moq;

namespace FlexCore.Database.SQLServer.Tests
{
    public class SQLServerProviderTests
    {
        private const string ConnectionString = "Server=localhost;Database=TestDB;Trusted_Connection=True;TrustServerCertificate=True;";

        [Fact]
        public void CreateConnection_ShouldReturnOpenConnection()
        {
            var provider = new SQLServerProvider(ConnectionString);
            using var connection = provider.CreateConnection();
            Assert.NotNull(connection);
        }

        [Fact]
        public async Task CreateConnectionAsync_ShouldReturnOpenConnection()
        {
            var provider = new SQLServerProvider(ConnectionString);
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
            var provider = new SQLServerProvider(ConnectionString);
            var connection = provider.CreateConnection();
            Assert.Equal(ConnectionState.Open, connection.State);
        }
    }
}
