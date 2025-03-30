// Program.cs
using FlexCore.Logging.Factory;
using FlexCore.Logging.Interfaces;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddLoggingProvider("Serilog"); // o "Console", "Log4Net"

var provider = services.BuildServiceProvider();
var logger = provider.GetRequiredService<ILoggingProvider>();

logger.Info("Questo è un messaggio informativo");
logger.Error("Questo è un errore simulato");