using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using FlexCore.Core.Configuration.Adapter;

class Program
{
    static void Main()
    {
        // Legge la variabile d'ambiente impostata da MSBuild
        string? resourcesFolder = Environment.GetEnvironmentVariable("RESOURCES_FOLDER");

        if (string.IsNullOrWhiteSpace(resourcesFolder))
        {
            Console.WriteLine("Errore: Variabile d'ambiente RESOURCES_FOLDER non trovata.");
            return;
        }

        Console.WriteLine($"Cartella di lavoro: {resourcesFolder}");

        // Percorso del file di configurazione
        string configPath = Path.Combine(resourcesFolder, "appsettings.json");

        if (!File.Exists(configPath))
        {
            Console.WriteLine($"Errore: Il file di configurazione non esiste in {configPath}");
            return;
        }

        // Carica la configurazione
        var config = new ConfigurationBuilder()
            .SetBasePath(resourcesFolder) 
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var appConfig = new ConfigurationAdapter(config);

        var dbSettings = appConfig.GetDatabaseSettings();
        Console.WriteLine($"Provider DB predefinito: {dbSettings.DefaultProvider}");

        var cacheSettings = appConfig.GetCacheSettings();
        Console.WriteLine($"Dimensione massima cache: {cacheSettings.MemoryCache.SizeLimit}");

        Console.ReadLine();
    }
}
