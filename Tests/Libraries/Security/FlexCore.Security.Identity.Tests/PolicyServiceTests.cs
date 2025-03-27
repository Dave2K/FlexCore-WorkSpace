using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace FlexCore.Security.Identity.Services.Tests
{
    public class PolicyServiceTests
    {
        private static IAuthorizationService CreateAuthService()
        {
            var services = new ServiceCollection();
            services.AddAuthorization(PolicyService.ConfigurePolicies);
            services.AddLogging();
            return services.BuildServiceProvider().GetRequiredService<IAuthorizationService>();
        }

        [Fact]
        public async Task AdminOnlyPolicy_ValidAdmin_Success()
        {
            var authService = CreateAuthService();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, "Admin")
            }));

            var result = await authService.AuthorizeAsync(user, null, "AdminOnly");
            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task ContentEditorPolicy_ValidRoles_Success()
        {
            var authService = CreateAuthService();

            foreach (var role in new[] { "Admin", "Editor" })
            {
                var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Role, role)
                }));
                var result = await authService.AuthorizeAsync(user, null, "ContentEditor");
                Assert.True(result.Succeeded);
            }
        }
    }
}