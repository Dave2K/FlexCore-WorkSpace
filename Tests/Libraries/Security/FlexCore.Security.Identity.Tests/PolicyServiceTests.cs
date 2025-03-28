using FlexCore.Security.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace FlexCore.Security.Identity.Tests
{
    public class PolicyServiceTests
    {
        [Fact]
        public void ConfigurePolicies_AddsCorrectPolicies()
        {
            // Arrange
            var options = new AuthorizationOptions();

            // Act
            PolicyService.ConfigurePolicies(options);

            // Assert
            var adminPolicy = options.GetPolicy("AdminOnly");
            Assert.NotNull(adminPolicy);

            var roleRequirement = adminPolicy.Requirements
                .OfType<ClaimsAuthorizationRequirement>()
                .FirstOrDefault(r => r.ClaimType == ClaimTypes.Role);

            Assert.NotNull(roleRequirement);
            Assert.True(roleRequirement.AllowedValues?.Contains("Admin") ?? false);

            Assert.NotNull(options.GetPolicy("ContentEditor"));
        }
    }
}