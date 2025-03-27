using Xunit;
using System;
using FlexCore.Logging.Core;

namespace FlexCore.Logging.Core.Tests
{
    /// <summary>
    /// Test per la classe LoggingProviderBase
    /// </summary>
    public class LoggingProviderBaseTests
    {
        /// <summary>
        /// Verifica che il provider registri il messaggio corretto.
        /// </summary>
        [Fact]
        public void Log_ShouldStoreMessage()
        {
            var provider = new TestLoggingProvider();
            provider.Info("Test message");

            Assert.Equal("[INFO] Test message", provider.LastMessage);
        }

        /// <summary>
        /// Verifica che il provider gestisca correttamente i messaggi null.
        /// </summary>
        [Fact]
        public void Log_NullMessage_ShouldNotThrow()
        {
            var provider = new TestLoggingProvider();

            var exception = Record.Exception(() => provider.Error(null!));

            Assert.Null(exception);
            Assert.Equal("[ERROR] ", provider.LastMessage);
        }

        /// <summary>
        /// Implementazione di test per LoggingProviderBase
        /// </summary>
        private class TestLoggingProvider : LoggingProviderBase
        {
            public string LastMessage { get; private set; } = string.Empty;

            public override void Debug(string message) => LastMessage = $"[DEBUG] {message}";
            public override void Info(string message) => LastMessage = $"[INFO] {message}";
            public override void Warn(string message) => LastMessage = $"[WARN] {message}";
            public override void Error(string message) => LastMessage = $"[ERROR] {message}";
            public override void Fatal(string message) => LastMessage = $"[FATAL] {message}";
        }
    }
}
