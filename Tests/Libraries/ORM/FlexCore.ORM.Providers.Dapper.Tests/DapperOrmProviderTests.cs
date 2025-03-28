using Xunit;
using Microsoft.Data.Sqlite;
using Dapper; // Aggiungi questa direttiva
using Dapper.Contrib.Extensions; // Aggiungi questa direttiva
using FlexCore.ORM.Core.Interfaces;
using System;

namespace FlexCore.ORM.Providers.Dapper.Tests;

public class DapperOrmProviderTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DapperOrmProvider _provider;

    public DapperOrmProviderTests()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        // Usa Dapper per eseguire la query
        _connection.Execute(@"
            CREATE TABLE TestEntity (
                Id TEXT PRIMARY KEY, 
                Name TEXT NOT NULL
            )");

        _provider = new DapperOrmProvider(_connection);
    }

    [Fact]
    public async Task FullCrudOperation_Success()
    {
        var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Test" };

        // Insert
        await _provider.AddAsync(entity);
        var retrieved = await _provider.GetByIdAsync<TestEntity>(entity.Id);
        Assert.Equal("Test", retrieved?.Name);

        // Update
        entity.Name = "Updated";
        await _provider.UpdateAsync(entity);
        var updated = await _provider.GetByIdAsync<TestEntity>(entity.Id);
        Assert.Equal("Updated", updated?.Name);

        // Delete
        await _provider.DeleteAsync(entity);
        var deleted = await _provider.GetByIdAsync<TestEntity>(entity.Id);
        Assert.Null(deleted);
    }

    public void Dispose()
    {
        _connection?.Close();
        _provider.Dispose();
        GC.SuppressFinalize(this);
    }

    [Table("TestEntity")]
    public class TestEntity
    {
        [ExplicitKey] // Da Dapper.Contrib.Extensions
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}