using System;
using System.Data;
using System.IO;
using Microsoft.Extensions.Configuration;
using FlexCore.Database.Factory;

/// <summary>
/// Punto di ingresso principale dell'applicazione demo per il database.
/// </summary>
public class Program
{
    /// <summary>
    /// Metodo principale che configura l'applicazione ed esegue operazioni sul database.
    /// </summary>
    /// <param name="args">Argomenti della riga di comando (non utilizzati in questo esempio).</param>
    public static void Main(string[] args)
    {
        // Configurazione dell'applicazione
        var configuration = BuildConfiguration();

        // Inizializzazione della factory (✅ Corregge CS0103)
        //var factory = new DatabaseConnectionFactory(configuration);

        //try
        //{
        //    ExecuteDatabaseOperations(factory);
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Errore critico: {ex.Message}");
        //}
    }

    /// <summary>
    /// Costruisce la configurazione dell'applicazione leggendo appsettings.json.
    /// </summary>
    /// <returns>Istanza di <see cref="IConfiguration"/>.</returns>
    private static IConfiguration BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) // ✅ Percorso affidabile
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        return builder.Build();
    }

    /// <summary>
    /// Esegue operazioni di esempio sul database utilizzando la factory fornita.
    /// </summary>
    /// <param name="factory">Factory per la creazione di connessioni al database.</param>
    //private static void ExecuteDatabaseOperations(DatabaseConnectionFactory factory)
    //{
    //    // Operazioni con SQLite (✅ Corregge CS8417 e CS1061)
    //    using (var connection = factory.CreateConnection("SQLite"))
    //    {
    //        connection.Open();
    //        using (var command = connection.CreateCommand())
    //        {
    //            command.CommandText = "SELECT COUNT(*) FROM Users";
    //            var result = command.ExecuteScalar(); // ✅ Metodo sincrono
    //            Console.WriteLine($"Utenti nel database: {result}");
    //        }
    //    }

    //    // Operazioni con SQL Server
    //    using (var connection = factory.CreateConnection("SQLServer"))
    //    {
    //        connection.Open();
    //        using (var command = connection.CreateCommand())
    //        {
    //            command.CommandText = "SELECT GETDATE()";
    //            var result = command.ExecuteScalar();
    //            Console.WriteLine($"Data corrente sul server: {result}");
    //        }
    //    }
    //}
}