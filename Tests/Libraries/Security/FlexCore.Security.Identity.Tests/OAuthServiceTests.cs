using FlexCore.Security.Identity.Models;
using FlexCore.Security.Identity.Services;
using Moq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace FlexCore.Security.Identity.Tests
{
    public class OAuthServiceTests
    {
        [Fact]
        public async Task AuthenticateWithGoogleAsync_ValidToken_ReturnsPrincipalWithClaims()
        {
            // Arrange
            var mockValidator = new Mock<IGoogleTokenValidator>();
            mockValidator.Setup(v => v.ValidateAsync("valid-token"))
                .ReturnsAsync(new GoogleTokenPayload
                {
                    Subject = "123",
                    Email = "test@example.com",
                    Name = "Test User"
                });

            var service = new OAuthService(mockValidator.Object);

            // Act
            var principal = await service.AuthenticateWithGoogleAsync("valid-token");

            // Assert
            Assert.Contains(principal.Claims, c =>
                c.Type == ClaimTypes.Email && c.Value == "test@example.com");
            Assert.Equal("Google", principal.Identity?.AuthenticationType);
        }
    }
}