using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FlexCore.Core.Configuration.Models;
using FlexCore.Core.Configuration.Extensions;

namespace FlexCore.Core.Configuration.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddDatabaseSettings_ShouldRegisterDatabaseSettings()
    {
        var services = new ServiceCollection();
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"DatabaseSettings:DefaultProvider", "SQLServer"},
                {"DatabaseSettings:SQLServer:ConnectionString", "valid_conn_string"}
            })
            .Build();
        var loggerMock = new Mock<ILogger>();

        services.AddDatabaseSettings(config, loggerMock.Object);

        var provider = services.BuildServiceProvider();
        var dbSettings = provider.GetService<DatabaseSettings>();

        Assert.NotNull(dbSettings);
        Assert.Equal("SQLServer", dbSettings.DefaultProvider);
    }

    [Fact]
    public void AddDatabaseSettings_ShouldThrowOnMissingConfiguration()
    {
        var services = new ServiceCollection();
        var config = new ConfigurationBuilder().Build(); // Configurazione vuota
        var loggerMock = new Mock<ILogger>();

        Assert.Throws<InvalidOperationException>(() =>
            services.AddDatabaseSettings(config, loggerMock.Object));
    }
}