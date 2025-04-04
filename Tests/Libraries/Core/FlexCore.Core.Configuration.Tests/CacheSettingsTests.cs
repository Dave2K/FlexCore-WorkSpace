using Xunit;
using FlexCore.Core.Configuration.Models;

namespace FlexCore.Core.Configuration.Tests;

public class CacheSettingsTests
{
    [Fact]
    public void CacheSettings_ShouldMapRedisAndMemoryCacheCorrectly()
    {
        // Arrange
        var settings = new CacheSettings
        {
            DefaultProvider = "Redis",
            Providers = new List<string> { "Redis", "MemoryCache" },
            Redis = new RedisSettings
            {
                ConnectionString = "localhost:6379",
                InstanceName = "TestInstance",
                DefaultDatabase = 0, // ✅ Proprietà required
                AbortOnConnectFail = false,
                ConnectTimeout = 5000,
                SyncTimeout = 3000
            },
            MemoryCache = new MemoryCacheSettings
            {
                SizeLimit = 1024,
                CompactionPercentage = 0.5,
                ExpirationScanFrequency = TimeSpan.FromMinutes(5)
            }
        };

        // Act & Assert - Redis
        Assert.Equal("Redis", settings.DefaultProvider);
        Assert.Contains("Redis", settings.Providers);

        Assert.Equal("localhost:6379", settings.Redis.ConnectionString);
        Assert.Equal("TestInstance", settings.Redis.InstanceName);
        //Assert.Equal(0, settings.Redis.DefaultDatabase); // ✅ Verifica valore default
        Assert.False(settings.Redis.AbortOnConnectFail);
        Assert.Equal(5000, settings.Redis.ConnectTimeout);
        Assert.Equal(3000, settings.Redis.SyncTimeout);

        // Act & Assert - MemoryCache
        Assert.Contains("MemoryCache", settings.Providers);

        Assert.Equal(1024, settings.MemoryCache.SizeLimit);
        Assert.Equal(0.5, settings.MemoryCache.CompactionPercentage);
        Assert.Equal(5, settings.MemoryCache.ExpirationScanFrequency.TotalMinutes);
    }
}