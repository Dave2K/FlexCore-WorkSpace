namespace FlexCore.Logging.Console;

using FlexCore.Logging.Core;
using FlexCore.Logging.Interfaces;

/// <summary>
/// Provider di logging su console.
/// </summary>
public class ConsoleLogger : BaseLogger, ILoggingProvider
{
    /// <summary>
    /// Registra un messaggio su console.
    /// </summary>
    /// <param name="level">Livello di log.</param>
    /// <param name="message">Messaggio da registrare.</param>
    protected override void Log(string level, string message)
    {
        System.Console.WriteLine($"[{level}] {message}");
    }
}