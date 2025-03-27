using Xunit;
using System;
using System.IO;
using FlexCore.Logging.Console;

namespace FlexCore.Logging.Console.Tests;

/// <summary>
/// Test unitari per la classe <see cref="ConsoleLogger"/>.
/// </summary>
public class ConsoleLoggerTests
{
    /// <summary>
    /// Verifica che il logger scriva correttamente su console.
    /// </summary>
    [Fact]
    public void ConsoleLogger_Should_Log_Correctly()
    {
        // Reindirizza l'output della console
        using var sw = new StringWriter();
        System.Console.SetOut(sw);

        var logger = new ConsoleLogger();

        // Esegue il log di prova
        logger.Debug("Test Debug");
        logger.Info("Test Info");
        logger.Warn("Test Warn");
        logger.Error("Test Error");
        logger.Fatal("Test Fatal");

        // Legge l'output e verifica
        var output = sw.ToString();
        Assert.Contains("[DEBUG] Test Debug", output);
        Assert.Contains("[INFO] Test Info", output);
        Assert.Contains("[WARN] Test Warn", output);
        Assert.Contains("[ERROR] Test Error", output);
        Assert.Contains("[FATAL] Test Fatal", output);
    }
}
