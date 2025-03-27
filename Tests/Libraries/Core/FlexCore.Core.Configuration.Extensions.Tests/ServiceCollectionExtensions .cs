using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using FlexCore.Core.Configuration.Extensions;
using FlexCore.Core.Configuration.Models;
using System.Collections.Generic;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddAppSettings_RegistersAppSettings()
    {
        var services = new ServiceCollection();
        var configData = new List<KeyValuePair<string, string?>>
        {
            new("ConnectionStrings:DefaultDatabase", "TestDB"),
            new("DatabaseSettings:DefaultProvider", "SQLServer"),
            new("Logging:Enabled", "true")
        };
        var config = new ConfigurationBuilder().AddInMemoryCollection(configData).Build();

        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<AppSettings>();
        services.AddAppSettings(config, logger);
        var serviceProvider = services.BuildServiceProvider();

        var options = serviceProvider.GetService<IOptions<AppSettings>>();
        Assert.NotNull(options);
        Assert.NotNull(options.Value);
        Assert.Equal("TestDB", options.Value.ConnectionStrings.DefaultDatabase);
        Assert.True(options.Value.Logging.Enabled);
    }
}
