using Xunit;
using System;
using FlexCore.Caching.Common.Validators;
using FlexCore.Caching.Factory;
using FlexCore.Caching.Interfaces;

namespace FlexCore.Caching.Common.Tests;

public class CacheKeyValidatorTests
{
    [Fact]
    public void ValidateKey_ThrowsArgumentException_WhenKeyIsNullOrEmpty()
    {
        Assert.Throws<ArgumentException>(() => CacheKeyValidator.ValidateKey(null!));
        Assert.Throws<ArgumentException>(() => CacheKeyValidator.ValidateKey(""));
        Assert.Throws<ArgumentException>(() => CacheKeyValidator.ValidateKey("   "));
    }

    [Fact]
    public void ValidateKey_DoesNotThrow_WhenKeyIsValid()
    {
        var exception = Record.Exception(() => CacheKeyValidator.ValidateKey("valid_key"));
        Assert.Null(exception);
    }

    [Fact]
    public void RegisterProvider_ThrowsArgumentNullException_WhenProviderFactoryIsNull()
    {
        var factory = new CacheProviderFactory();
        Func<ICacheProvider>? providerFactory = null!;
        Assert.Throws<ArgumentNullException>(() => factory.RegisterProvider("test", providerFactory!));
    }
}
