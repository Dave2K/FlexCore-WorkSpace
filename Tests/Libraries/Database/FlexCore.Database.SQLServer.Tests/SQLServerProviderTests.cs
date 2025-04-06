using FlexCore.Database.SQLServer;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace FlexCore.Database.SQLServer.Tests
{
    /// <summary>
    /// Fixture per la gestione centralizzata del database di test SQL Server
    /// </summary>
    public class SQLServerTestFixture : IDisposable
    {
        private const string TestDatabaseName = "FlexCoreTestDB";
        private readonly string _masterConnectionString = @"Server=.\SQLEXPRESS;Database=master;Trusted_Connection=True;Encrypt=False;";

        /// <summary>
        /// Ottiene il provider di database configurato
        /// </summary>
        public SQLServerDatabaseProvider Provider { get; }

        /// <summary>
        /// Ottiene la stringa di connessione per il database di test
        /// </summary>
        public string TestConnectionString { get; }

        /// <summary>
        /// Inizializza una nuova istanza della fixture
        /// </summary>
        public SQLServerTestFixture()
        {
            Provider = new SQLServerDatabaseProvider();
            TestConnectionString = $@"Server=.\SQLEXPRESS;Database={TestDatabaseName};Trusted_Connection=True;Encrypt=False;";

            CreateTestDatabase();
            Provider.Connect(TestConnectionString);
        }

        /// <summary>
        /// Inizializza lo stato del database per un test specifico
        /// </summary>
        public void Initialize(string testTable)
        {
            Provider.ExecuteNonQuery(
                $@"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{testTable}')
                BEGIN
                    CREATE TABLE {testTable} (
                        Id INT PRIMARY KEY,
                        Data NVARCHAR(50)
                    )
                END"
            );
        }

        /// <summary>
        /// Esegue la pulizia delle risorse
        /// </summary>
        public void Dispose()
        {
            CleanupTestTables();
            Provider.Disconnect();
            DropTestDatabase();
            GC.SuppressFinalize(this);
        }

        private void CreateTestDatabase()
        {
            using (var connection = new SqlConnection(_masterConnectionString))
            {
                connection.Open();
                new SqlCommand(
                    $@"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{TestDatabaseName}')
                    BEGIN
                        CREATE DATABASE {TestDatabaseName}
                    END",
                    connection
                ).ExecuteNonQuery();
            }
        }

        private void DropTestDatabase()
        {
            using (var connection = new SqlConnection(_masterConnectionString))
            {
                connection.Open();
                new SqlCommand(
                    $@"IF EXISTS (SELECT * FROM sys.databases WHERE name = '{TestDatabaseName}')
                    BEGIN
                        ALTER DATABASE {TestDatabaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                        DROP DATABASE {TestDatabaseName}
                    END",
                    connection
                ).ExecuteNonQuery();
            }
        }

        private void CleanupTestTables()
        {
            Provider.ExecuteNonQuery(
                @"DECLARE @sql NVARCHAR(MAX) = '';
                SELECT @sql += 'DROP TABLE ' + QUOTENAME(name) + ';' 
                FROM sys.tables 
                WHERE name LIKE 'TestTable_%';
                EXEC sp_executesql @sql;"
            );
        }
    }
}