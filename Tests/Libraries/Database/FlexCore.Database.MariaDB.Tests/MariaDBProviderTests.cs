using Xunit;
using FlexCore.Database.MariaDB;
using MySqlConnector;
using System.Data;
using System.Threading.Tasks;
using WorkSpace.Generated;

namespace FlexCore.Database.MariaDB.Tests
{
    /// <summary>
    /// Test avanzati per <see cref="MariaDBDatabaseProvider"/> con gestione transazionale.
    /// </summary>
    public class MariaDBProviderTests : IClassFixture<MariaDBTestFixture>
    {
        private readonly MariaDBTestFixture _fixture;
        private const string TestTable = "TestTable_123";

        public MariaDBProviderTests(MariaDBTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.Initialize();
        }

        /// <summary>
        /// Verifica il ciclo completo CRUD con parametrizzazione.
        /// </summary>
        [Fact]
        public void FullCrudCycle_WithParameters_ShouldPersistData()
        {
            var provider = _fixture.Provider;

            // Insert
            provider.ExecuteNonQuery(
                $"INSERT INTO {TestTable} (Id, Content) VALUES (@Id, @Content);",
                provider.CreateParameter("@Id", 1),
                provider.CreateParameter("@Content", "TestContent")
            );

            // Read
            using var reader = provider.ExecuteQuery(
                $"SELECT Content FROM {TestTable} WHERE Id = @Id;",
                provider.CreateParameter("@Id", 1)
            );

            Assert.True(reader.Read());
            Assert.Equal("TestContent", reader["Content"]);

            // Delete
            var affectedRows = provider.ExecuteNonQuery(
                $"DELETE FROM {TestTable} WHERE Id = @Id;",
                provider.CreateParameter("@Id", 1)
            );

            Assert.Equal(1, affectedRows);
        }

        /// <summary>
        /// Verifica il rollback esplicito di una transazione.
        /// </summary>
        [Fact]
        public void ExplicitTransactionRollback_ShouldDiscardChanges()
        {
            var provider = _fixture.Provider;

            provider.BeginTransaction();
            provider.ExecuteNonQuery($"INSERT INTO {TestTable} (Id) VALUES (999);");
            provider.RollbackTransaction();

            using var reader = provider.ExecuteQuery($"SELECT Id FROM {TestTable} WHERE Id = 999;");
            Assert.False(reader.Read());
        }

        /// <summary>
        /// Verifica la crezione di parametri con valori nulli.
        /// </summary>
        [Fact]
        public void CreateParameter_WithNullValue_ShouldHandleCorrectly()
        {
            var provider = _fixture.Provider;
            var param = provider.CreateParameter("@nullParam", null);

            Assert.IsType<MySqlParameter>(param);
            Assert.Equal(DBNull.Value, param.Value);
        }
    }

    /// <summary>
    /// Fixture per gestione connessione e pulizia del database.
    /// </summary>
    public class MariaDBTestFixture : IDisposable
    {
        public MariaDBDatabaseProvider Provider { get; } = new();
        private const string TestTable = "TestTable_123";

        public void Initialize()
        {
            Provider.Connect("Server=localhost;Database=test;User=root;Password=;");
            Provider.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS {TestTable} (Id INT PRIMARY KEY, Content TEXT);");
        }

        public void Dispose()
        {
            Provider.ExecuteNonQuery($"DROP TABLE IF EXISTS {TestTable};");
            Provider.Disconnect();
            GC.SuppressFinalize(this);
        }
    }
}