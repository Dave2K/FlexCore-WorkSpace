using Xunit;
using FlexCore.Core.Configuration;
using Microsoft.Extensions.Configuration;
using System;

public class ConfigurationAdapterTests
{
    [Fact]
    public void GetValue_ReturnsValue_WhenKeyExists()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new[] { new KeyValuePair<string, string?>("key", "value") })
            .Build();
        var adapter = new ConfigurationAdapter(config);
        var result = adapter.GetValue("key");
        Assert.Equal("value", result);
    }

    [Fact]
    public void GetValue_ReturnsNull_WhenKeyNotExists()
    {
        var config = new ConfigurationBuilder().Build();
        var adapter = new ConfigurationAdapter(config);
        var result = adapter.GetValue("nonexistent_key");
        Assert.Null(result);
    }

    [Fact]
    public void GetAppSettings_ThrowsInvalidOperationException_WhenSettingsNotConfigured()
    {
        var config = new ConfigurationBuilder().Build();
        var adapter = new ConfigurationAdapter(config);
        Assert.Throws<InvalidOperationException>(() => adapter.GetAppSettings());
    }

    [Fact]
    public void GetConnectionStrings_ThrowsInvalidOperationException_WhenSettingsNotConfigured()
    {
        var config = new ConfigurationBuilder().Build();
        var adapter = new ConfigurationAdapter(config);
        Assert.Throws<InvalidOperationException>(() => adapter.GetConnectionStrings());
    }
}