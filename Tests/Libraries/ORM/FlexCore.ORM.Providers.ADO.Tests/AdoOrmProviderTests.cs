using FlexCore.ORM.Core.Interfaces;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xunit;
using SQLitePCL;

namespace FlexCore.ORM.Providers.ADO.Tests
{
    public class AdoOrmProviderTests : IDisposable
    {
        private readonly SqliteConnection _conn;
        private readonly AdoOrmProvider _provider;
        private bool _disposed;

        public AdoOrmProviderTests()
        {
            Batteries.Init();
            _conn = new SqliteConnection("Data Source=:memory:");
            _conn.Open();

            using var cmd = _conn.CreateCommand();
            cmd.CommandText = "CREATE TABLE TestEntity(Id TEXT PRIMARY KEY, Name TEXT NOT NULL)";
            cmd.ExecuteNonQuery();

            _provider = new AdoOrmProvider(_conn);
        }

        [Fact]
        public async Task AddAndRetrieveEntity_Success()
        {
            var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Test" };
            await _provider.AddAsync(entity);
            var result = await _provider.GetByIdAsync<TestEntity>(entity.Id);
            Assert.Equal(entity.Name, result?.Name);
        }

        public class TestEntity
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        public class AdoOrmProvider(SqliteConnection conn) : IOrmProvider
        {
            public async Task AddAsync<T>(T entity) where T : class
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO TestEntity (Id, Name) VALUES (@Id, @Name)";
                cmd.Parameters.AddWithValue("@Id", ((dynamic)entity).Id);
                cmd.Parameters.AddWithValue("@Name", ((dynamic)entity).Name);
                await cmd.ExecuteNonQueryAsync();
            }

            public async Task<T?> GetByIdAsync<T>(Guid id) where T : class
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT Name FROM TestEntity WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", id);

                using var reader = await cmd.ExecuteReaderAsync();
                return reader.Read() ?
                    new TestEntity { Id = id, Name = reader.GetString(0) } as T :
                    null;
            }

            // Implementazioni corrette dell'interfaccia
            public Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class => Task.CompletedTask;
            public Task UpdateAsync<T>(T entity) where T : class => Task.CompletedTask;
            public Task UpdateRangeAsync<T>(IEnumerable<T> entities) where T : class => Task.CompletedTask;
            public Task DeleteAsync<T>(T entity) where T : class => Task.CompletedTask;
            public Task DeleteRangeAsync<T>(IEnumerable<T> entities) where T : class => Task.CompletedTask;
            public Task<IEnumerable<T>> GetAllAsync<T>() where T : class => Task.FromResult(Enumerable.Empty<T>());
            public Task<int> SaveChangesAsync() => Task.FromResult(0);
            public Task BeginTransactionAsync() => Task.CompletedTask;
            public Task CommitTransactionAsync() => Task.CompletedTask;
            public Task RollbackTransactionAsync() => Task.CompletedTask;

            public void Dispose() => conn.Dispose();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _provider.Dispose();
            _conn.Close();
            _conn.Dispose();
            _disposed = true;
        }
    }
}