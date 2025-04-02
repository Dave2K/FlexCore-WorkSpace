using Xunit;
using FlexCore.Database.SQLServer;
using System;

namespace FlexCore.Database.SQLServer.Tests
{
    public class SQLServerProviderTests
    {
        private const string TestConnectionString = "Server=.;Database=tempdb;Trusted_Connection=True;";

        [Fact]
        public void CreateConnection_ValidString_ReturnsSqlConnection()
        {
            var provider = new SQLServerDatabaseProvider();
            var connection = provider.CreateConnection(TestConnectionString);
            Assert.IsType<Microsoft.Data.SqlClient.SqlConnection>(connection);
        }

        [Fact]
        public async Task OpenConnectionAsync_ValidConnection_OpensSuccessfully()
        {
            var provider = new SQLServerDatabaseProvider();
            var connection = provider.CreateConnection(TestConnectionString);
            await provider.OpenConnectionAsync(connection);
            Assert.Equal(System.Data.ConnectionState.Open, connection.State);
        }
    }
}