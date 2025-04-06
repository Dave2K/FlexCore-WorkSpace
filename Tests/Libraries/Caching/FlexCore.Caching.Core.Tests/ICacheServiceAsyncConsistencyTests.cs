using FlexCore.Caching.Core.Interfaces;
using FlexCore.Caching.Memory;
using FlexCore.Caching.Redis;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FlexCore.Caching.Core.Tests
{
    /// <summary>
    /// Verifica la conformità async pura dell'interfaccia <see cref="ICacheService"/>
    /// e l'assenza di metodi sincroni
    /// </summary>
    public class ICacheServiceAsyncConsistencyTests
    {
        /// <summary>
        /// Verifica che l'interfaccia contenga solo metodi asincroni
        /// </summary>
        [Fact]
        public void Interface_ShouldContainOnlyAsyncMethods()
        {
            // Arrange
            var methodNames = typeof(ICacheService).GetMethods()
                .Select(m => m.Name)
                .ToList();

            // Act
            var syncMethods = methodNames
                .Where(name => !name.EndsWith("Async"))
                .ToList();

            // Assert
            Assert.Empty(syncMethods);
        }

        /// <summary>
        /// Verifica che tutte le implementazioni rispettino il pattern async
        /// </summary>
        [Theory]
        [InlineData(typeof(MemoryCacheProvider))]
        [InlineData(typeof(RedisCacheProvider))]
        public void Implementations_ShouldOverrideOnlyAsyncMethods(Type providerType)
        {
            // Arrange
            var asyncMethods = typeof(ICacheService).GetMethods()
                .Where(m => m.Name.EndsWith("Async"))
                .Select(m => m.Name)
                .ToList();

            var providerMethods = providerType.GetMethods()
                .Where(m => m.DeclaringType == providerType)
                .Select(m => m.Name)
                .ToList();

            // Act
            var invalidMethods = providerMethods
                .Where(name => !asyncMethods.Contains(name))
                .ToList();

            // Assert
            Assert.Empty(invalidMethods);
        }

        /// <summary>
        /// Verifica che i metodi async ritornino Task
        /// </summary>
        [Theory]
        [InlineData(typeof(MemoryCacheProvider))]
        [InlineData(typeof(RedisCacheProvider))]
        public void AsyncMethods_ShouldReturnTask(Type providerType)
        {
            // Arrange/Act
            var methods = providerType.GetMethods()
                .Where(m => m.Name.EndsWith("Async")
                    && m.ReturnType != typeof(Task)
                    && !m.ReturnType.IsSubclassOf(typeof(Task)))
                .ToList();

            // Assert
            Assert.Empty(methods);
        }
    }
}