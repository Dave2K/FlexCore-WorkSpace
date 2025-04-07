using FlexCore.Caching.Core.Interfaces;
using FlexCore.Caching.Memory;
using FlexCore.Caching.Redis;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace FlexCore.Caching.Core.Tests
{
    /// <summary>
    /// Verifica la conformità asincrona delle implementazioni ICacheService
    /// </summary>
    /// <remarks>
    /// Garantisce che tutte le implementazioni rispettino il pattern async puro
    /// e non introducano metodi sincroni non necessari
    /// </remarks>
    public class ICacheServiceAsyncConsistencyTests
    {
        /// <summary>
        /// Verifica che l'interfaccia contenga solo metodi asincroni
        /// </summary>
        [Fact]
        public void Interface_ShouldContainOnlyAsyncMethods()
        {
            // Arrange
            var methods = typeof(ICacheService).GetMethods();

            // Act
            var syncMethods = methods
                .Where(m => !m.Name.EndsWith("Async"))
                .ToList();

            // Assert
            Assert.Empty(syncMethods);
        }

        /// <summary>
        /// Verifica che le implementazioni non sovrascrivano metodi non async
        /// </summary>
        /// <param name="providerType">Tipo del provider da testare</param>
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

            // Act
            var providerMethods = providerType.GetMethods(
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    BindingFlags.DeclaredOnly
                )
                .Where(m =>
                    !m.IsSpecialName &&
                    m.DeclaringType == providerType &&
                    m.GetBaseDefinition().DeclaringType == typeof(ICacheService)
                )
                .Select(m => m.Name)
                .ToList();

            var invalidMethods = providerMethods
                .Where(name => !asyncMethods.Contains(name))
                .ToList();

            // Assert
            Assert.Empty(invalidMethods);
        }

        /// <summary>
        /// Verifica che i metodi async ritornino Task
        /// </summary>
        /// <param name="providerType">Tipo del provider da testare</param>
        [Theory]
        [InlineData(typeof(MemoryCacheProvider))]
        [InlineData(typeof(RedisCacheProvider))]
        public void AsyncMethods_ShouldReturnTask(Type providerType)
        {
            // Arrange
            var methods = providerType.GetMethods()
                .Where(m => m.Name.EndsWith("Async"))
                .ToList();

            // Act
            var invalidMethods = methods
                .Where(m =>
                    m.ReturnType != typeof(Task) &&
                    !m.ReturnType.IsSubclassOf(typeof(Task))
                )
                .ToList();

            // Assert
            Assert.Empty(invalidMethods);
        }
    }
}