using Xunit;
using Moq;
using FlexCore.Database.Interfaces;
using System;
using System.Threading.Tasks;

namespace FlexCore.Database.Factory.Tests
{
    /// <summary>
    /// Test per verificare il corretto funzionamento della classe <see cref="DatabaseProviderFactory"/>.
    /// </summary>
    public class DatabaseProviderFactoryTests
    {
        /// <summary>
        /// Verifica che la factory restituisca un provider registrato correttamente.
        /// </summary>
        [Fact]
        public void CreateProvider_ShouldReturnRegisteredProvider()
        {
            var factory = new DatabaseProviderFactory();
            factory.RegisterProvider("SQLServer", (conn) => new Mock<IDbConnectionFactory>().Object);
            var provider = factory.CreateProvider("SQLServer", "dummy_connection");
            Assert.NotNull(provider);
        }

        /// <summary>
        /// Verifica che la factory sollevi un'eccezione per provider non registrati.
        /// </summary>
        [Fact]
        public void CreateProvider_ShouldThrowIfNotRegistered()
        {
            var factory = new DatabaseProviderFactory();
            Assert.Throws<NotSupportedException>(() =>
                factory.CreateProvider("InvalidProvider", "dummy_connection"));
        }
    }
}