using Xunit;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using FluentAssertions;

namespace FlexCore.Config.Tests
{
    /// <summary>
    /// Test suite per la validazione del file di configurazione principale
    /// </summary>
    public class AppSettingsValidationTests : IDisposable
    {
        private const string ConfigFileName = "appsettings.json";
        private readonly string _configFilePath;
        private IConfiguration? _configuration;

        public AppSettingsValidationTests()
        {
            _configFilePath = Path.Combine(
                WorkSpace.Generated.WSEnvironment.ResourcesFolder,
                ConfigFileName
            );
            BuildConfiguration();
        }

        [Fact]
        public void ConfigFile_Exists_ShouldReturnTrue()
        {
            File.Exists(_configFilePath).Should().BeTrue(
                $"il file di configurazione {ConfigFileName} deve esistere nel percorso: {_configFilePath}");
        }

        [Fact]
        public void ConfigFile_HasAllRequiredSections_ShouldBeValid()
        {
            var requiredSections = new[] {
                "DatabaseSettings",
                "CacheSettings",
                "Logging",
                "SecuritySettings"
            };

            var missingSections = requiredSections
                .Where(s => _configuration!.GetSection(s).Exists() == false)
                .ToList();

            missingSections.Should().BeEmpty(
                $"Sezioni mancanti nel file di configurazione: {string.Join(", ", missingSections)}");
        }

        [Fact]
        public void ConfigFile_DatabaseSettings_ShouldHaveValidConnectionStrings()
        {
            var dbSettings = _configuration!.GetSection("DatabaseSettings");

            dbSettings["DefaultProvider"].Should().NotBeNullOrEmpty(
                "DefaultProvider è obbligatorio");

            var providers = new[] { "SQLServer", "SQLite", "MariaDB" };
            foreach (var provider in providers)
            {
                var connectionString = dbSettings.GetSection(provider)["ConnectionString"];
                connectionString.Should().NotBeNullOrEmpty(
                    $"ConnectionString mancante per {provider}");

                if (provider == "SQLServer")
                {
                    connectionString.Should().Contain("Server=",
                        "Formato connection string non valido per SQLServer");
                }
            }
        }

        [Fact]
        public void ConfigFile_CacheSettings_ShouldHaveValidConfiguration()
        {
            var cacheSettings = _configuration!.GetSection("CacheSettings");

            cacheSettings["DefaultProvider"].Should().BeOneOf(
                new[] { "Redis", "MemoryCache" },
                "DefaultProvider della cache non valido");

            cacheSettings.GetSection("Redis")["ConnectionString"].Should().NotBeNullOrEmpty(
                "ConnectionString Redis mancante");

            cacheSettings.GetSection("MemoryCache")["SizeLimit"].Should().NotBeNullOrEmpty(
                "SizeLimit per MemoryCache mancante");
        }

        [Fact]
        public void ConfigFile_MissingFile_ShouldThrowException()
        {
            var invalidPath = Path.Combine(
                WorkSpace.Generated.WSEnvironment.ResourcesFolder,
                "invalid.json"
            );

            Action act = () => new ConfigurationBuilder()
                .AddJsonFile(invalidPath, optional: false)
                .Build();

            act.Should().Throw<FileNotFoundException>();
        }

        [Fact]
        public void ConfigFile_InvalidJson_ShouldThrowException()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "{ \"invalid\": "); // JSON malformato intenzionalmente

            // Act & Assert
            Action act = () => new ConfigurationBuilder()
                .AddJsonFile(tempFile)
                .Build();

            // Modifica qui: Cattura l'eccezione esterna e verifica l'inner exception
            act.Should()
                .Throw<InvalidDataException>() // Eccezione effettivamente generata
                .WithInnerException<FormatException>(); // Eccezione originale di parsing

            // Cleanup
            File.Delete(tempFile);
        }

        private void BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(WorkSpace.Generated.WSEnvironment.ResourcesFolder)
                .AddJsonFile(
                    path: ConfigFileName,
                    optional: false,
                    reloadOnChange: true
                );

            _configuration = builder.Build();
        }

        public void Dispose()
        {
            // Cleanup esplicito per scenari di test avanzati
            _configuration = null;
            GC.SuppressFinalize(this);
        }
    }
}