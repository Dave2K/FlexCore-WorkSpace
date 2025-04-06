using FlexCore.Database.Core.Interfaces;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FlexCore.Database.Core.Tests
{
    /// <summary>
    /// Classe base astratta per test comuni su tutti i provider di database.
    /// Fornisce test standard per operazioni CRUD, transazioni e gestione parametri.
    /// </summary>
    public abstract class BaseDatabaseProviderTests : IClassFixture<IBaseTestFixture>, IDisposable
    {
        protected readonly IDatabaseProvider _provider;
        protected abstract string TestTable { get; }
        protected abstract IDatabaseProvider Provider { get; }

        protected BaseDatabaseProviderTests(IBaseTestFixture fixture)
        {
            _provider = fixture.Provider;
            fixture.Initialize(TestTable);
        }

        /// <summary>
        /// Verifica che una query SELECT restituisca i dati inseriti.
        /// </summary>
        [Fact]
        public void ExecuteQuery_AfterInsert_ShouldReturnData()
        {
            // Arrange
            _provider.ExecuteNonQuery($"INSERT INTO {TestTable} (Id, Name) VALUES (1, 'Test');");

            // Act
            using var reader = _provider.ExecuteQuery($"SELECT Name FROM {TestTable} WHERE Id = 1;");

            // Assert
            Assert.True(reader.Read());
            Assert.Equal("Test", reader["Name"]);
        }

        /// <summary>
        /// Verifica che una transazione committata salvi i dati.
        /// </summary>
        [Fact]
        public void CommitTransaction_ShouldPersistData()
        {
            // Arrange
            _provider.BeginTransaction();

            // Act
            _provider.ExecuteNonQuery($"INSERT INTO {TestTable} (Id) VALUES (999);");
            _provider.CommitTransaction();

            // Assert
            using var reader = _provider.ExecuteQuery($"SELECT Id FROM {TestTable} WHERE Id = 999;");
            Assert.True(reader.Read());
        }

        /// <summary>
        /// Verifica che un parametro creato con valore nullo sia gestito correttamente.
        /// </summary>
        [Fact]
        public void CreateParameter_WithNullValue_ShouldSetDbNull()
        {
            // Act
            var param = _provider.CreateParameter("@testParam", null);

            // Assert
            Assert.Equal(DBNull.Value, param.Value);
        }

        /// <summary>
        /// Verifica che ExecuteScalarAsync gestisca correttamente i valori nulli.
        /// </summary>
        [Fact]
        public async Task ExecuteScalarAsync_WithNullValue_ShouldThrow()
        {
            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _provider.ExecuteScalarAsync<int>($"SELECT Name FROM {TestTable} WHERE Id = -1;"));
        }

        public void Dispose()
        {
            _provider.ExecuteNonQuery($"DELETE FROM {TestTable};");
            _provider.Disconnect();
        }
    }

    /// <summary>
    /// Interfaccia per le fixture di test comuni.
    /// </summary>
    public interface IBaseTestFixture : IDisposable
    {
        void Initialize(string testTable);
        IDatabaseProvider Provider { get; }
    }
}