using Xunit;
using FlexCore.Database.SQLServer;
using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace FlexCore.Database.SQLServer.Tests
{
    public class SQLServerProviderTests
    {
        private static readonly string TestConnectionString;

        // Costruttore statico per inizializzare TestConnectionString in modo sicuro
        static SQLServerProviderTests()
        {
            string configPath = Path.Combine(
                WorkSpace.Generated.WSEnvironment.ResourcesFolder,
                "appsettings.json"
            );

            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException($"File di configurazione non trovato: {configPath}");
            }

            string json = File.ReadAllText(configPath);
            var config = JObject.Parse(json);

            TestConnectionString = config["DatabaseSettings"]?["SQLServer"]?["ConnectionString"]?.ToString()
                                  ?? throw new InvalidOperationException("La connection string non può essere null.");

            if (string.IsNullOrWhiteSpace(TestConnectionString))
            {
                throw new InvalidOperationException("La connection string è vuota.");
            }
        }

        [Fact]
        public void CreateConnection_ValidString_ReturnsSqlConnection()
        {
            var provider = new SQLServerDatabaseProvider();
            var connection = provider.CreateConnection(TestConnectionString);
            Assert.IsType<Microsoft.Data.SqlClient.SqlConnection>(connection);
        }

        [Fact]
        public async Task OpenConnectionAsync_ValidConnection_OpensSuccessfully()
        {
            var provider = new SQLServerDatabaseProvider();
            var connection = provider.CreateConnection(TestConnectionString);
            await provider.OpenConnectionAsync(connection);
            Assert.Equal(System.Data.ConnectionState.Open, connection.State);
        }
    }
}
