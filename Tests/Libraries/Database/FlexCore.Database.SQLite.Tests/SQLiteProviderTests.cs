using Xunit;
using FlexCore.Database.SQLite;
using Microsoft.Data.Sqlite;
using System;
using System.IO;
using WorkSpace.Generated;

namespace FlexCore.Database.SQLite.Tests
{
    /// <summary>
    /// Test completi per <see cref="SQLiteDatabaseProvider"/>.
    /// </summary>
    public class SQLiteProviderTests : IClassFixture<SQLiteTestFixture>
    {
        private readonly SQLiteTestFixture _fixture;
        private const string TestTable = "TestTable_456";

        public SQLiteProviderTests(SQLiteTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.Initialize(TestTable);
        }

        /// <summary>
        /// Verifica la creazione di parametri con valori nulli.
        /// </summary>
        [Fact]
        public void CreateParameter_WithNullValue_ShouldHandleCorrectly()
        {
            var provider = _fixture.Provider;
            // Correzione: Utilizza DBNull.Value invece di null
            var param = provider.CreateParameter("@nullParam", DBNull.Value); // Risolve CS8625

            Assert.IsType<SqliteParameter>(param);
            Assert.Equal(DBNull.Value, param.Value);
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
    }

    /// <summary>
    /// Fixture per la gestione del database SQLite in memoria.
    /// </summary>
    public class SQLiteTestFixture : IDisposable
    {
        public SQLiteDatabaseProvider Provider { get; } = new();
        private const string TestTable = "TestTable_456";

        public void Initialize(string testTable)
        {
            Provider.Connect("Data Source=:memory:");
            Provider.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS {testTable} (Id INT PRIMARY KEY, Content TEXT);");
        }

        public void Dispose()
        {
            Provider.ExecuteNonQuery($"DROP TABLE IF EXISTS {TestTable};");
            Provider.Disconnect();
            GC.SuppressFinalize(this);
        }
    }
}