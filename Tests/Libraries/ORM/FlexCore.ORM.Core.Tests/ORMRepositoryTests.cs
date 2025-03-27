namespace FlexCore.ORM.Core.Tests
{
    using Xunit;
    using FlexCore.ORM.Core;
    using System;
    using System.Collections.Generic;

    public class ORMRepositoryTests
    {
        [Fact]
        public void Insert_ShouldAddEntity()
        {
            // Arrange
            var repository = new ORMRepository<TestEntity>();
            var entity = new TestEntity { Id = 1, Name = "Test" };

            // Act
            repository.Insert(entity);
            var result = repository.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public void Update_ShouldModifyEntity()
        {
            // Arrange
            var repository = new ORMRepository<TestEntity>();
            var entity = new TestEntity { Id = 1, Name = "Old Name" };
            repository.Insert(entity);

            // Act
            entity.Name = "New Name";
            repository.Update(entity);
            var result = repository.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Name", result.Name);
        }

        [Fact]
        public void Delete_ShouldRemoveEntity()
        {
            // Arrange
            var repository = new ORMRepository<TestEntity>();
            var entity = new TestEntity { Id = 1, Name = "Test" };
            repository.Insert(entity);

            // Act
            repository.Delete(1);
            var result = repository.GetById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAll_ShouldReturnAllEntities()
        {
            // Arrange
            var repository = new ORMRepository<TestEntity>();
            repository.Insert(new TestEntity { Id = 1, Name = "Entity1" });
            repository.Insert(new TestEntity { Id = 2, Name = "Entity2" });

            // Act
            var result = repository.GetAll();

            // Assert
            Assert.Equal(2, result.Count);
        }
    }

    public class ORMRepository<T> where T : class
    {
        private readonly Dictionary<int, T> _storage = new();

        public void Insert(T entity)
        {
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null)
            {
                int id = (int)idProperty.GetValue(entity)!;
                _storage[id] = entity;
            }
        }

        public void Update(T entity)
        {
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null)
            {
                int id = (int)idProperty.GetValue(entity)!;
                if (_storage.ContainsKey(id))
                {
                    _storage[id] = entity;
                }
            }
        }

        public void Delete(int id)
        {
            _storage.Remove(id);
        }

        public T? GetById(int id) => _storage.TryGetValue(id, out var entity) ? entity : null;

        public List<T> GetAll() => _storage.Values.ToList();
    }

    public class TestEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
