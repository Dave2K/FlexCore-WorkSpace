using Xunit;
using FlexCore.Database.SQLite;
using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace FlexCore.Database.SQLite.Tests
{
    public class SQLiteProviderTests
    {
        private const string ConnectionString = "Data Source=:memory:";

        [Fact]
        public void CreateConnection_ShouldReturnOpenConnection()
        {
            var provider = new SQLiteProvider(ConnectionString);
            using var connection = provider.CreateConnection();
            Assert.Equal(ConnectionState.Open, connection.State);
        }

        [Fact]
        public async Task CreateConnectionAsync_ShouldReturnOpenConnection()
        {
            var provider = new SQLiteProvider(ConnectionString);
            using var connection = await provider.CreateConnectionAsync();
            Assert.Equal(ConnectionState.Open, connection.State);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenConnectionStringIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new SQLiteProvider(null));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenConnectionStringIsEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => new SQLiteProvider(""));
        }
    }
}
