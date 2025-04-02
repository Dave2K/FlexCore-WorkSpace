using Xunit;
using FlexCore.Database.MariaDB;
using MySqlConnector;

namespace FlexCore.Database.MariaDB.Tests
{
    public class MariaDBProviderTests
    {
        [Fact]
        public void CreateConnection_ValidString_ReturnsMySqlConnection()
        {
            var provider = new MariaDBDatabaseProvider();
            var connection = provider.CreateConnection("Server=test;Database=test;User=test;");
            Assert.IsType<MySqlConnection>(connection);
        }
    }
}