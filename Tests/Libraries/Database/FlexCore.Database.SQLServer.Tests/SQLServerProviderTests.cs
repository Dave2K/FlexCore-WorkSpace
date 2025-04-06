using FlexCore.Database.SQLServer;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Transactions;
using Xunit;

namespace FlexCore.Database.SQLServer.Tests
{
    /// <summary>
    /// Test completi per <see cref="SQLServerDatabaseProvider"/> con gestione transazionale.
    /// </summary>
    public class SQLServerProviderTests : IClassFixture<SQLServerTestFixture>
    {
        private readonly SQLServerTestFixture _fixture;
        private const string TestTable = "TestTable_789";

        public SQLServerProviderTests(SQLServerTestFixture fixture)
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
            var param = provider.CreateParameter("@nullParam", DBNull.Value);

            Assert.IsType<SqlParameter>(param);
            Assert.Equal(DBNull.Value, param.Value);
        }

        /// <summary>
        /// Verifica il corretto funzionamento delle transazioni distribuite.
        /// </summary>
        [Fact]
        public void DistributedTransaction_ShouldRollbackAcrossOperations()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var provider = _fixture.Provider;
                provider.ExecuteNonQuery($"INSERT INTO {TestTable} (Id) VALUES (999);");
                // Non chiamare Complete() per rollback
            }

            using var reader = _fixture.Provider.ExecuteQuery($"SELECT Id FROM {TestTable} WHERE Id = 999;");
            Assert.False(reader.Read());
        }
    }

    /// <summary>
    /// Fixture per la gestione del database SQL Server.
    /// </summary>
    public class SQLServerTestFixture : IDisposable
    {
        public SQLServerDatabaseProvider Provider { get; }
        private const string TestDatabaseName = "FlexCoreTestDB";
        private string _currentTestTable;

        public SQLServerTestFixture()
        {
            Provider = new SQLServerDatabaseProvider();
            Provider.Connect($"Server=.\\SQLEXPRESS;Database={TestDatabaseName};Trusted_Connection=True;Encrypt=False;");
        }

        public void Initialize(string testTable)
        {
            _currentTestTable = testTable;
            Provider.ExecuteNonQuery(
                $@"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{_currentTestTable}')
                BEGIN
                    CREATE TABLE {_currentTestTable} (Id INT PRIMARY KEY, Data NVARCHAR(50))
                END"
            );
        }

        public void Dispose()
        {
            Provider.ExecuteNonQuery(
                $@"IF EXISTS (SELECT * FROM sys.tables WHERE name = '{_currentTestTable}')
                BEGIN
                    DROP TABLE {_currentTestTable}
                END"
            );
            Provider.Disconnect();
            GC.SuppressFinalize(this);
        }
    }
}