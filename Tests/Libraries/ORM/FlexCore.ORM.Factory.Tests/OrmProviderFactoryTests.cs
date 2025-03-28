namespace FlexCore.ORM.Factory.Tests
{
    using Xunit;
    using FlexCore.ORM.Core.Interfaces;
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class OrmProviderFactoryTests
    {
        [Fact]
        public void CreateProvider_WithValidName_ReturnsProvider()
        {
            var factory = new OrmProviderFactory();
            factory.RegisterProvider("ADO", _ => new MockOrmProvider());
            var provider = factory.CreateProvider("ADO", "connection_string");
            Assert.IsAssignableFrom<IOrmProvider>(provider);
        }

        [Fact]
        public void CreateProvider_WithInvalidName_ThrowsException()
        {
            var factory = new OrmProviderFactory();
            Assert.Throws<NotSupportedException>(() =>
                factory.CreateProvider("Invalid", "connection_string"));
        }
    }

    public class MockOrmProvider : IOrmProvider
    {
        public Task<T?> GetByIdAsync<T>(Guid id) where T : class => Task.FromResult<T?>(null);
        public Task<IEnumerable<T>> GetAllAsync<T>() where T : class => Task.FromResult(Enumerable.Empty<T>());
        public Task AddAsync<T>(T entity) where T : class => Task.CompletedTask;
        public Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class => Task.CompletedTask;
        public Task UpdateAsync<T>(T entity) where T : class => Task.CompletedTask;
        public Task UpdateRangeAsync<T>(IEnumerable<T> entities) where T : class => Task.CompletedTask;
        public Task DeleteAsync<T>(T entity) where T : class => Task.CompletedTask;
        public Task DeleteRangeAsync<T>(IEnumerable<T> entities) where T : class => Task.CompletedTask;
        public Task<int> SaveChangesAsync() => Task.FromResult(0);
        public Task BeginTransactionAsync() => Task.CompletedTask;
        public Task CommitTransactionAsync() => Task.CompletedTask;
        public Task RollbackTransactionAsync() => Task.CompletedTask;
        public void Dispose() { }
    }
}