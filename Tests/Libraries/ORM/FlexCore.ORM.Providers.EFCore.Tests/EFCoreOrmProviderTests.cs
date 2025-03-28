namespace FlexCore.ORM.Providers.EFCore.Tests;

using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public class EFCoreOrmProviderTests
{
    private TestDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new TestDbContext(options);
    }

    [Fact]
    public async Task AddAsync_ShouldInsertEntity()
    {
        var dbContext = CreateDbContext();
        var provider = new EFCoreOrmProvider(dbContext);
        var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Test" };

        await provider.AddAsync(entity);
        var result = await provider.GetByIdAsync<TestEntity>(entity.Id);

        Assert.Equal("Test", result?.Name);
    }

    public class TestDbContext : DbContext
    {
        public DbSet<TestEntity> Entities { get; set; } = null!;
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
    }

    public class TestEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}