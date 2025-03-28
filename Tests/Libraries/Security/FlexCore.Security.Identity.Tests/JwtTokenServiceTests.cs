using FlexCore.Security.Identity.Models;
using FlexCore.Security.Identity.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace FlexCore.Security.Identity.Tests
{
    public class JwtTokenServiceTests
    {
        [Fact]
        public void GenerateToken_ValidClaims_ReturnsValidJwt()
        {
            // Arrange
            var settings = new JwtSettings
            {
                SecretKey = "supersecretkeythatshouldbelongenough",
                Issuer = "test-issuer",
                Audience = "test-audience",
                ExpiryMinutes = 60
            };

            var service = new JwtTokenService(settings);
            var claims = new[] { new Claim(ClaimTypes.Name, "test-user") };

            // Act
            var token = service.GenerateToken(claims);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            Assert.Equal("test-issuer", jwt.Issuer);
            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Name && c.Value == "test-user");
        }
    }
}