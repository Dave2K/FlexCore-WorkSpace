using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FlexCore.Security.Identity.Models;
using FlexCore.Security.Identity.Services;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace FlexCore.Security.Identity.Tests
{
    public class JwtTokenServiceTests
    {
        private readonly JwtSettings _settings = new()
        {
            SecretKey = "supersecretkey1234567890supersecretkey",
            Issuer = "test-issuer",
            Audience = "test-audience",
            ExpiryMinutes = 30
        };

        [Fact]
        public void GenerateToken_ShouldReturnValidJwt()
        {
            // Arrange
            var service = new JwtTokenService(_settings);
            var claims = new[] { new Claim(ClaimTypes.Name, "testuser") };

            // Act
            var token = service.GenerateToken(claims);

            // Assert
            Assert.NotEmpty(token);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            Assert.Equal(_settings.Issuer, jwt.Issuer);
            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Name && c.Value == "testuser");
            Assert.True(jwt.ValidTo > DateTime.UtcNow.AddMinutes(29));
        }

        [Theory]
        [InlineData(15)]
        [InlineData(60)]
        public void GenerateToken_ShouldRespectExpiryTime(int minutes)
        {
            // Correzione per CS8858
            var customSettings = new JwtSettings
            {
                SecretKey = _settings.SecretKey,
                Issuer = _settings.Issuer,
                Audience = _settings.Audience,
                ExpiryMinutes = minutes
            };

            var service = new JwtTokenService(customSettings);
            var token = service.GenerateToken(Array.Empty<Claim>());
            var jwt = new JwtSecurityToken(token);

            var expectedExpiry = DateTime.UtcNow.AddMinutes(minutes);
            Assert.True(jwt.ValidTo < expectedExpiry.AddSeconds(5));
            Assert.True(jwt.ValidTo > expectedExpiry.AddSeconds(-5));
        }
    }
}