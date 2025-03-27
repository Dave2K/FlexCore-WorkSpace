// File: FlexCore.Security.Identity.Tests/OAuthServiceTests.cs
using System.Security.Claims;
using System.Threading.Tasks;
using FlexCore.Security.Identity.Models;
using FlexCore.Security.Identity.Services;
using Moq;
using Xunit;

namespace FlexCore.Security.Identity.Tests
{
    public class OAuthServiceTests
    {
        private readonly Mock<IGoogleTokenValidator> _validatorMock = new();

        [Fact]
        public async Task AuthenticateWithGoogle_ShouldReturnValidPrincipal()
        {
            // Arrange
            var expectedPayload = new GoogleTokenPayload
            {
                Subject = "123",
                Email = "test@example.com",
                Name = "Test User"
            };

            _validatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<string>()))
                .ReturnsAsync(expectedPayload);

            var service = new OAuthService(_validatorMock.Object);

            // Act
            var principal = await service.AuthenticateWithGoogleAsync("any-token");

            // Assert
            Assert.Equal(expectedPayload.Subject, principal.FindFirstValue(ClaimTypes.NameIdentifier));
            Assert.Equal(expectedPayload.Email, principal.FindFirstValue(ClaimTypes.Email));
            Assert.Equal(expectedPayload.Name, principal.FindFirstValue(ClaimTypes.Name));
        }
    }
}