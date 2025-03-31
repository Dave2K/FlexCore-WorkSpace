using Microsoft.Extensions.Configuration;
using Xunit;
using FlexCore.Core.Configuration.Adapter;
using System.Collections.Generic;

namespace FlexCore.Core.Configuration.Tests;

public class ConfigurationAdapterTests
{
    [Fact]
    public void GetDatabaseSettings_ShouldReturnValidSettings()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            {"DatabaseSettings:DefaultProvider", "SQLServer"},
            {"DatabaseSettings:SQLServer:ConnectionString", "Server=test;"},
            {"DatabaseSettings:SQLite:ConnectionString", "Data Source=test.db;"}
        };

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var adapter = new ConfigurationAdapter(config);
        var dbSettings = adapter.GetDatabaseSettings();

        Assert.Equal("SQLServer", dbSettings.DefaultProvider);
        Assert.Equal("Server=test;", dbSettings.SQLServer.ConnectionString);
    }

    [Fact]
    public void GetLoggingSettings_ShouldDeserializeCorrectly()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            {"Logging:DefaultProvider", "Serilog"},
            {"Logging:Serilog:MinimumLevel:Default", "Information"}
        };

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var adapter = new ConfigurationAdapter(config);
        var loggingSettings = adapter.GetLoggingSettings();

        Assert.Equal("Serilog", loggingSettings.DefaultProvider);
        Assert.Equal("Information", loggingSettings.Serilog.MinimumLevel.Default);
    }
}