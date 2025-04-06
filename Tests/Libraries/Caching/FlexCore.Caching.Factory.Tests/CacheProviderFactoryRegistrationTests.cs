// File: CacheProviderFactoryRegistrationTests.cs
using FlexCore.Caching.Core.Interfaces;
using FlexCore.Caching.Factory; // Aggiungere il riferimento mancante
using Moq;
using Xunit;
using System;

namespace FlexCore.Caching.Factory.Tests // Correggere il namespace
{
    /// <summary>
    /// Test suite per le operazioni di registrazione dei provider
    /// </summary>
    public class CacheProviderFactoryRegistrationTests
    {
        /// <summary>
        /// Verifica il comportamento con nome provider nullo
        /// </summary>
        [Fact]
        public void RegisterProvider_NullName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var factory = new CacheProviderFactory();
            var mockProvider = new Mock<ICacheProvider>().Object;

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(
                () => factory.RegisterProvider(null!, () => mockProvider)
            );

            Assert.Equal("name", ex.ParamName);
        }

        /// <summary>
        /// Verifica il comportamento con factory method nullo
        /// </summary>
        [Fact]
        public void RegisterProvider_NullCreator_ShouldThrowArgumentNullException()
        {
            // Arrange
            var factory = new CacheProviderFactory();

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(
                () => factory.RegisterProvider("nullProvider", null!)
            );

            Assert.Equal("creator", ex.ParamName);
        }

        /// <summary>
        /// Verifica la sovrascrittura di provider esistenti
        /// </summary>
        [Fact]
        public void CreateCacheProvider_AfterOverwrite_ShouldReturnLastRegistered()
        {
            // Arrange
            var factory = new CacheProviderFactory();
            var firstProvider = new Mock<ICacheProvider>().Object;
            var secondProvider = new Mock<ICacheProvider>().Object;

            // Act
            factory.RegisterProvider("test", () => firstProvider);
            factory.RegisterProvider("test", () => secondProvider);

            // Assert
            var result = factory.CreateCacheProvider("test");
            Assert.Same(secondProvider, result);
        }

        /// <summary>
        /// Verifica l'eccezione per provider non registrato
        /// </summary>
        [Fact]
        public void CreateCacheProvider_UnregisteredName_ShouldThrowArgumentException()
        {
            // Arrange
            var factory = new CacheProviderFactory();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(
                () => factory.CreateCacheProvider("ghostProvider")
            );

            Assert.Contains("non registrato", ex.Message);
            Assert.Equal("name", ex.ParamName);
        }
    }
}