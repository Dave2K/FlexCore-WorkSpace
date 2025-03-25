using Xunit;
using System;
using FlexCore.Logging.Core;
using FlexCore.Logging.Factory;
using FlexCore.Logging.Interfaces;

namespace FlexCore.Logging.Core.Tests
{
    /// <summary>
    /// Test per la classe LoggingProviderFactory
    /// </summary>
    public class LoggingProviderFactoryTests
    {
        /// <summary>
        /// Verifica che il factory registri e restituisca correttamente un provider.
        /// </summary>
        [Fact]
        public void RegisterProvider_ShouldStoreAndReturnProvider()
        {
            var factory = new LoggingProviderFactory();
            factory.RegisterProvider("TestProvider", () => new TestLoggingProvider());

            var provider = factory.CreateProvider("TestProvider");

            Assert.NotNull(provider);
            Assert.IsType<TestLoggingProvider>(provider);
        }

        /// <summary>
        /// Verifica che la creazione di un provider non registrato generi un'eccezione.
        /// </summary>
        [Fact]
        public void CreateProvider_UnregisteredProvider_ShouldThrowException()
        {
            var factory = new LoggingProviderFactory();

            Assert.Throws<NotSupportedException>(() => factory.CreateProvider("NonExistent"));
        }

        /// <summary>
        /// Implementazione di test per ILoggingProvider
        /// </summary>
        private class TestLoggingProvider : ILoggingProvider
        {
            public void Debug(string message) { }
            public void Info(string message) { }
            public void Warn(string message) { }
            public void Error(string message) { }
            public void Fatal(string message) { }
            public void Log(string message) { }
        }
    }
}
