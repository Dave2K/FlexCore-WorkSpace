namespace FlexCore.ORM.Providers.EFCore
{
    using Xunit;
    using FlexCore.ORM.Providers.EFCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.InMemory;
    using System;
    using System.Threading.Tasks;
    using System.Linq;

    public class EFCoreOrmProviderTests
    {
        private TestDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Database univoco per ogni test
                .Options;
            return new TestDbContext(options);
        }

        private class TestDbContext : DbContext
        {
            public DbSet<TestEntity> Entities { get; set; } = null!;
            public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
        }

        private class TestEntity
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        [Fact]
        public async Task AddAsync_ShouldInsertEntity()
        {
            var dbContext = CreateDbContext();
            var provider = new EFCoreOrmProvider(dbContext);
            var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Test" };

            await provider.AddAsync(entity);
            var result = await provider.GetByIdAsync<TestEntity>(entity.Id);

            Assert.NotNull(result);
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyEntity()
        {
            var dbContext = CreateDbContext();
            var provider = new EFCoreOrmProvider(dbContext);
            var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Old Name" };

            await provider.AddAsync(entity);
            entity.Name = "New Name";
            await provider.UpdateAsync(entity);
            var result = await provider.GetByIdAsync<TestEntity>(entity.Id);

            Assert.NotNull(result);
            Assert.Equal("New Name", result.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveEntity()
        {
            var dbContext = CreateDbContext();
            var provider = new EFCoreOrmProvider(dbContext);
            var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Test" };

            await provider.AddAsync(entity);
            await provider.DeleteAsync(entity);
            var result = await provider.GetByIdAsync<TestEntity>(entity.Id);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            var dbContext = CreateDbContext();
            var provider = new EFCoreOrmProvider(dbContext);
            await provider.AddAsync(new TestEntity { Id = Guid.NewGuid(), Name = "Entity1" });
            await provider.AddAsync(new TestEntity { Id = Guid.NewGuid(), Name = "Entity2" });

            var result = await provider.GetAllAsync<TestEntity>();

            Assert.Equal(2, result.Count());
        }
    }
}