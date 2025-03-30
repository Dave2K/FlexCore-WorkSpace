// Program.cs
using FlexCore.Database.Factory.Extensions;
using FlexCore.Database.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// Configurazione SQL Server
services.AddDatabaseProvider(
    "SQLServer",
    "Server=localhost;Database=TestDB;User Id=sa;Password=YourPassword;");

var provider = services.BuildServiceProvider();
var dbFactory = provider.GetRequiredService<IDbConnectionFactory>();

using var connection = dbFactory.CreateConnection();
var command = connection.CreateCommand();
command.CommandText = "SELECT 1";
var result = command.ExecuteScalar();
Console.WriteLine($"Risultato query: {result}");