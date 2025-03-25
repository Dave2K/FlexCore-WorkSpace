using Xunit;
using FlexCore.Core.Configuration.Models;

namespace FlexCore.Core.Configuration.Models.Tests;

public class CacheSettingsTests
{
    [Fact]
    public void CacheSettings_Should_Initialize_Correctly()
    {
        var settings = new CacheSettings
        {
            DefaultProvider = "MemoryCache",
            MemoryCache = new MemoryCacheSettings { SizeLimit = 1024, CompactionPercentage = 0.2, ExpirationScanFrequency = TimeSpan.FromMinutes(5) },
            Redis = new RedisSettings { ConnectionString = "localhost", InstanceName = "TestInstance", DefaultDatabase = 0, AbortOnConnectFail = false, ConnectTimeout = 5000, SyncTimeout = 5000 }
        };

        Assert.Equal("MemoryCache", settings.DefaultProvider);
    }
}
