using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;
using Moq;
using FlexCore.Core.Configuration.Models;
using FlexCore.Core.Configuration.Extensions;

namespace FlexCore.Core.Configuration.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddAppSettings_RegistersAllSettings()
        {
            // Arrange
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> // <-- Nullable fix
                {
                    {"AppSettings:ConnectionStrings:DefaultDatabase", "TestDB_Connection"},
                    {"ConnectionStrings:DefaultDatabase", "TestDB_Connection"},
                    {"DatabaseSettings:DefaultProvider", "SQLServer"},
                    {"ORMSettings:DefaultProvider", "EntityFramework"},
                    {"CacheSettings:DefaultProvider", "MemoryCache"}
                })
                .Build();

            var loggerMock = new Mock<ILogger>();

            // Act
            services.AddAppSettings(configuration, loggerMock.Object);

            // Assert
            var serviceProvider = services.BuildServiceProvider();

            // 1. Verifica AppSettings
            var appSettingsOptions = serviceProvider.GetService<IOptions<AppSettings>>();
            Assert.NotNull(appSettingsOptions);
            Assert.NotNull(appSettingsOptions.Value);
            Assert.Equal("TestDB_Connection", appSettingsOptions.Value.ConnectionStrings.DefaultDatabase);

            // 2. Verifica ConnectionStrings
            var connStringsOptions = serviceProvider.GetService<IOptions<ConnectionStringsSettings>>();
            Assert.NotNull(connStringsOptions);
            Assert.NotNull(connStringsOptions.Value);
            Assert.Equal("TestDB_Connection", connStringsOptions.Value.DefaultDatabase);

            // 3. Verifica DatabaseSettings
            var dbSettingsOptions = serviceProvider.GetService<IOptions<DatabaseSettings>>();
            Assert.NotNull(dbSettingsOptions);
            Assert.Equal("SQLServer", dbSettingsOptions.Value.DefaultProvider);

            // 4. Verifica ORMSettings
            var ormSettingsOptions = serviceProvider.GetService<IOptions<ORMSettings>>();
            Assert.NotNull(ormSettingsOptions);
            Assert.Equal("EntityFramework", ormSettingsOptions.Value.DefaultProvider);

            // 5. Verifica CacheSettings
            var cacheSettingsOptions = serviceProvider.GetService<IOptions<CacheSettings>>();
            Assert.NotNull(cacheSettingsOptions);
            Assert.Equal("MemoryCache", cacheSettingsOptions.Value.DefaultProvider);
        }
    }
}