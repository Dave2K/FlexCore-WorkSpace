using Xunit;
using FlexCore.Core.Configuration.Models;

namespace FlexCore.Core.Configuration.Tests;

public class SecuritySettingsTests
{
    [Fact]
    public void SecuritySettings_ShouldMapJwtAndOAuthCorrectly()
    {
        var settings = new SecuritySettings
        {
            DefaultProvider = "JWT",
            Providers = new List<string> { "JWT", "OAuth" },
            JWT = new JwtSettings
            {
                SecretKey = "test_key",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                ExpiryMinutes = 120
            },
            OAuth = new OAuthSettings
            {
                Google = new GoogleSettings
                {
                    ClientId = "google_test_id",
                    ClientSecret = "google_test_secret"
                },
                Facebook = new FacebookSettings
                {
                    ClientId = "facebook_test_id",
                    ClientSecret = "facebook_test_secret"
                }
            }
        };

        Assert.Equal("JWT", settings.DefaultProvider);
        Assert.Equal("TestIssuer", settings.JWT.Issuer);
        Assert.Equal("google_test_id", settings.OAuth.Google.ClientId);
        Assert.Equal(120, settings.JWT.ExpiryMinutes);
    }
}