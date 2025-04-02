using Xunit;
using FlexCore.Database.SQLite;
using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace FlexCore.Database.SQLite.Tests
{
    public class SQLiteProviderTests
    {
        [Fact]
        public void CreateConnection_ValidString_ReturnsSqliteConnection()
        {
            var provider = new SQLiteDatabaseProvider();
            DbConnection connection = provider.CreateConnection("Data Source=:memory:");
            Assert.IsType<SqliteConnection>(connection);
        }
    }
}