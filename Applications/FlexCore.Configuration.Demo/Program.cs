using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using FlexCore.Core.Configuration.Generated;
using FlexCore.Core.Configuration.Adapter;

string resourcesFolder = BuildConfig.ResourcesFolder;
Console.WriteLine($"Cartella di lavoro: {resourcesFolder}");

string configPath = Path.Combine(resourcesFolder, "appsettings.json");
if (!File.Exists(configPath))
{
    Console.WriteLine($"Errore: Il file di configurazione non esiste in {configPath}");
    return;
}

var config = new ConfigurationBuilder()
    .SetBasePath(resourcesFolder)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var appConfig = new ConfigurationAdapter(config);

// Sezione Database
var dbSettings = appConfig.GetDatabaseSettings();
Console.WriteLine("\n--- Database Settings ---");
Console.WriteLine($"Provider predefinito: {dbSettings.DefaultProvider}");
Console.WriteLine($"Provider supportati: {string.Join(", ", dbSettings.Providers)}");

// Sezione Cache
var cacheSettings = appConfig.GetCacheSettings();
Console.WriteLine("\n--- Cache Settings ---");
Console.WriteLine($"Provider predefinito: {cacheSettings.DefaultProvider}");
Console.WriteLine($"Provider supportati: {string.Join(", ", cacheSettings.Providers)}");

// Sezione Logging
var loggingSettings = appConfig.GetLoggingSettings();
Console.WriteLine("\n--- Logging Settings ---");
Console.WriteLine($"Provider predefinito: {loggingSettings.DefaultProvider}");
Console.WriteLine($"Provider supportati: {string.Join(", ", loggingSettings.Providers)}");

// Sezione ORM
var ormSettings = appConfig.GetORMSettings();
Console.WriteLine("\n--- ORM Settings ---");
Console.WriteLine($"Provider predefinito: {ormSettings.DefaultProvider}");
Console.WriteLine($"Provider supportati: {string.Join(", ", ormSettings.Providers)}");