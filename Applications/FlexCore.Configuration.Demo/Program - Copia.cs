//using Microsoft.Extensions.Configuration;
//using FlexCore.Core.Configuration.Adapter;
//using System;

//var config = new ConfigurationBuilder()
////.SetBasePath(Directory.GetCurrentDirectory())
//    .SetBasePath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"))
//    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//    .Build();

//var appConfig = new ConfigurationAdapter(config);

//var dbSettings = appConfig.GetDatabaseSettings();
//Console.WriteLine($"Provider DB predefinito: {dbSettings.DefaultProvider}");

//var cacheSettings = appConfig.GetCacheSettings();
//Console.WriteLine($"Dimensione massima cache: {cacheSettings.MemoryCache.SizeLimit}");

//Console.ReadLine();