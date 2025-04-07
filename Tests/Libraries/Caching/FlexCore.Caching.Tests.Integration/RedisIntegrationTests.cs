using FlexCore.Caching.Core.Interfaces;
using FlexCore.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WorkSpace.Generated; 
using Xunit;

namespace FlexCore.Caching.Tests.Integration
{
    /// <summary>
    /// Test end-to-end per l'integrazione con Redis utilizzando il percorso delle risorse della workspace
    /// </summary>
    /// <remarks>
    /// Verifica il corretto funzionamento del provider Redis in ambienti reali
    /// </remarks>
    public class RedisIntegrationTests : IAsyncLifetime
    {
        private readonly ICacheProvider _provider;
        private readonly string _connectionString;

        /// <summary>
        /// Inizializza una nuova istanza del test con configurazione da WSEnvironment
        /// </summary>
        public RedisIntegrationTests()
        {
            // Ottieni il percorso delle risorse dalla classe generata
            var configPath = WSEnvironment.ResourcesFolder;

            var config = new ConfigurationBuilder()
                .SetBasePath(configPath) // ✅ Utilizza il percorso generato
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _connectionString = config.GetConnectionString("Redis")
                ?? throw new InvalidOperationException("Stringa di connessione Redis mancante");

            _provider = new RedisCacheProvider(
                _connectionString,
                new LoggerFactory().CreateLogger<RedisCacheProvider>()
            );
        }

        /// <inheritdoc/>
        public async Task InitializeAsync() => await _provider.ClearAllAsync();

        /// <inheritdoc/>
        public async Task DisposeAsync()
        {
            await _provider.ClearAllAsync();
            (_provider as IDisposable)?.Dispose();
        }

        /// <summary>
        /// Verifica l'intero ciclo CRUD (Create, Read, Update, Delete)
        /// </summary>
        [Fact]
        public async Task FullCRUDCycle_ShouldWork()
        {
            // Arrange
            const string key = "crud_test_key";
            const string initialValue = "value1";
            const string updatedValue = "value2";

            // Act & Assert - Create + Read
            await _provider.SetAsync(key, initialValue, TimeSpan.FromMinutes(1));
            var value1 = await _provider.GetAsync<string>(key);
            Assert.Equal(initialValue, value1);

            // Act & Assert - Update + Read
            await _provider.SetAsync(key, updatedValue, TimeSpan.FromMinutes(1));
            var value2 = await _provider.GetAsync<string>(key);
            Assert.Equal(updatedValue, value2);

            // Act & Assert - Delete
            var deleteResult = await _provider.RemoveAsync(key);
            Assert.True(deleteResult);
            var exists = await _provider.ExistsAsync(key);
            Assert.False(exists);
        }
    }
}