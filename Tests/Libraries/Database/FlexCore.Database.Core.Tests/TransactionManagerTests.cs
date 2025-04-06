using Xunit;
using FlexCore.Database.Core.Interfaces;
using FlexCore.Database.SQLite;
using FlexCore.Database.MariaDB;
using System.Transactions;
using System.Threading.Tasks;
using FlexCore.Database.Interfaces;
using Moq;
using System.Data;

namespace FlexCore.Database.Core.Tests
{
    /// <summary>
    /// Test per verificare il comportamento di <see cref="TransactionManager"/>.
    /// Copre transazioni sincrone, asincrone e distribuite.
    /// </summary>
    public class TransactionManagerTests
    {
        // Configurazione per SQLite (in-memory)
        private const string SQLiteConnectionString = "Data Source=:memory:";

        // Configurazione per MariaDB (sostituire con valori validi)
        private const string MariaDBConnectionString = "Server=localhost;Database=test;User=root;Password=;";

        /// <summary>
        /// Verifica che una transazione asincrona venga avviata correttamente su SQLite.
        /// </summary>
        [Fact]
        public async Task BeginTransactionAsync_SQLite_ShouldStartTransaction()
        {
            // Arrange
            var provider = new SQLiteDatabaseProvider();
            provider.Connect(SQLiteConnectionString);
            var transactionManager = new TransactionManager(provider);

            // Act
            await transactionManager.BeginTransactionAsync();

            // Assert
            Assert.True(provider.IsTransactionActive());
        }

        /// <summary>
        /// Verifica il commit di una transazione su MariaDB con dati reali.
        /// </summary>
        [Fact]
        public async Task CommitTransactionAsync_MariaDB_ShouldPersistData()
        {
            // Arrange
            var provider = new MariaDBDatabaseProvider();
            provider.Connect(MariaDBConnectionString);
            var transactionManager = new TransactionManager(provider);

            // Act
            await transactionManager.BeginTransactionAsync();
            provider.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS TestTable (Id INT);");
            provider.ExecuteNonQuery("INSERT INTO TestTable (Id) VALUES (1);");
            await transactionManager.CommitTransactionAsync();

            // Assert
            using var result = provider.ExecuteQuery("SELECT * FROM TestTable;");
            Assert.True(result.Read()); // Verifica presenza almeno una riga
        }

        /// <summary>
        /// Verifica il rollback di una transazione distribuita.
        /// </summary>
        [Fact]
        public async Task DistributedTransaction_ShouldRollbackAcrossProviders()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Arrange
                var sqliteProvider = new SQLiteDatabaseProvider();
                sqliteProvider.Connect(SQLiteConnectionString);
                sqliteProvider.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS Test (Id INT);");

                var mariaDbProvider = new MariaDBDatabaseProvider();
                mariaDbProvider.Connect(MariaDBConnectionString);
                mariaDbProvider.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS Test (Id INT);");

                // Act
                sqliteProvider.ExecuteNonQuery("INSERT INTO Test VALUES (1);");
                mariaDbProvider.ExecuteNonQuery("INSERT INTO Test VALUES (1);");

                // Non chiamare scope.Complete() per simulare rollback
            }

            // Verifica rollback
            var sqliteCheck = new SQLiteDatabaseProvider();
            sqliteCheck.Connect(SQLiteConnectionString);
            using var sqliteReader = sqliteCheck.ExecuteQuery("SELECT COUNT(*) FROM Test;");
            sqliteReader.Read();
            Assert.Equal(0, sqliteReader.GetInt32(0));

            var mariaDbCheck = new MariaDBDatabaseProvider();
            mariaDbCheck.Connect(MariaDBConnectionString);
            using var mariaReader = mariaDbCheck.ExecuteQuery("SELECT COUNT(*) FROM Test;");
            mariaReader.Read();
            Assert.Equal(0, mariaReader.GetInt32(0));
        }
    }

    // Classe TransactionManager corretta
    public class TransactionManager
    {
        private readonly IDatabaseProvider _provider;

        public TransactionManager(IDatabaseProvider provider)
        {
            _provider = provider;
        }

        public async Task BeginTransactionAsync()
        {
            if (_provider is IDataContext dataContext)
                await dataContext.BeginTransactionAsync();
            else
                _provider.BeginTransaction();
        }

        public async Task CommitTransactionAsync()
        {
            if (_provider is IDataContext dataContext)
                await dataContext.CommitTransactionAsync();
            else
                _provider.CommitTransaction();
        }
    }
}