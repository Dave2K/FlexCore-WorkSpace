// Program.cs
using FlexCore.Caching.Factory;
using FlexCore.Caching.Interfaces;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// Configurazione da file (appsettings.json)
// "CacheSettings": {
//   "DefaultProvider": "Redis",
//   "Redis": {
//     "ConnectionString": "localhost:6379",
//     "InstanceName": "DemoApp"
//   }
// }
//services.AddCacheProvider("MemoryCache"); // "Redis" o "MemoryCache"

var provider = services.BuildServiceProvider();
var cache = provider.GetRequiredService<ICacheService>();

// Test operazioni
const string key = "test_key";
cache.Set(key, "Hello World", TimeSpan.FromMinutes(1));
var value = cache.Get<string>(key);
Console.WriteLine($"Valore dalla cache: {value}");
cache.Remove(key);