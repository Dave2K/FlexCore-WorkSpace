using Xunit;
using FlexCore.Database.SQLite;
using Microsoft.Data.Sqlite;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using WorkSpace.Generated;

namespace FlexCore.Database.SQLite.Tests
{
    /// <summary>
    /// Test completi per <see cref="SQLiteDatabaseProvider"/>.
    /// </summary>
    public class SQLiteProviderTests
    {
        private static readonly string TempFolder = WSEnvironment.TempsFolder;

        /// <summary>
        /// Verifica operazioni CRUD con transazioni esplicite.
        /// </summary>
        [Fact]
        public void CrudOperations_WithTransactions_ShouldPersistChanges()
        {
            string dbPath = Path.Combine(TempFolder, "test_crud.db");
            string connectionString = $"Data Source={dbPath};";

            var provider = new SQLiteDatabaseProvider();
            try
            {
                provider.Connect(connectionString);
                provider.ExecuteNonQuery("CREATE TABLE Test (Id INTEGER PRIMARY KEY, Name TEXT);");

                // Transazione esplicita
                provider.BeginTransaction();
                provider.ExecuteNonQuery("INSERT INTO Test (Id, Name) VALUES (1, 'Alpha');");
                provider.CommitTransaction();

                // Lettura dati
                using var reader = provider.ExecuteQuery("SELECT * FROM Test WHERE Id = 1;");
                Assert.True(reader.Read()); // Sostituito HasRows
                Assert.Equal("Alpha", reader["Name"]);
            }
            finally
            {
                provider.Disconnect();
                if (File.Exists(dbPath)) File.Delete(dbPath);
            }
        }

        /// <summary>
        /// Verifica il corretto handling di valori nulli.
        /// </summary>
        [Fact]
        public void ExecuteScalar_WithNullValue_ShouldThrowException()
        {
            var provider = new SQLiteDatabaseProvider();
            provider.Connect("Data Source=:memory:");
            provider.ExecuteNonQuery("CREATE TABLE Test (Value TEXT);");

            var exception = Assert.Throws<InvalidOperationException>(() =>
                provider.ExecuteScalar<string>("SELECT Value FROM Test;"));

            Assert.Equal("Il risultato della query è nullo", exception.Message);
        }

        /// <summary>
        /// Verifica l'apertura asincrona di connessioni esterne.
        /// </summary>
        [Fact]
        public async Task OpenConnectionAsync_WithExternalConnection_ShouldManageState()
        {
            var provider = new SQLiteDatabaseProvider();
            var connection = provider.CreateConnection("Data Source=:memory:");

            await provider.OpenConnectionAsync(connection);
            Assert.Equal(ConnectionState.Open, connection.State);

            // Verifica operazioni post-apertura
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "CREATE TABLE AsyncTest (Id INT);";
            cmd.ExecuteNonQuery();

            connection.Dispose();
        }
    }
}