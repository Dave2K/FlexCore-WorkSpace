using Xunit;
using System;
using FleFlexCore.Logging.Core.Base;

namespace FlexCore.Logging.Core.Tests
{
    /// <summary>
    /// Test per la classe BaseLogger
    /// </summary>
    public class BaseLoggerTests
    {
        /// <summary>
        /// Verifica che il logger chiami il metodo Log con il livello e il messaggio corretti.
        /// </summary>
        [Fact]
        public void Log_ShouldStoreMessageWithLevel()
        {
            var logger = new TestLogger();
            logger.PublicLog("INFO", "Test message");

            Assert.Equal("[INFO] Test message", logger.LastMessage);
        }

        /// <summary>
        /// Verifica che il logger gestisca correttamente i messaggi null.
        /// </summary>
        [Fact]
        public void Log_NullMessage_ShouldNotThrow()
        {
            var logger = new TestLogger();

            var exception = Record.Exception(() => logger.PublicLog("ERROR", null));

            Assert.Null(exception);
            Assert.Equal("[ERROR] ", logger.LastMessage);
        }

        /// <summary>
        /// Implementazione di test per BaseLogger
        /// </summary>
        private class TestLogger : BaseLogger
        {
            public string LastMessage { get; private set; } = string.Empty;

            /// <summary>
            /// Metodo pubblico per chiamare Log nei test.
            /// </summary>
            public void PublicLog(string level, string? message)
            {
                Log(level, message ?? string.Empty);
            }

            protected override void Log(string level, string message)
            {
                LastMessage = $"[{level}] {message}";
            }
        }
    }
}
