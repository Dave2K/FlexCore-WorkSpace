using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FlexCore.Security.Identity.Services
{
    public static class PolicyService
    {
        public static void ConfigurePolicies(AuthorizationOptions options)
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireClaim(ClaimTypes.Role, "Admin"));

            options.AddPolicy("ContentEditor", policy =>
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c =>
                        c.Type == ClaimTypes.Role &&
                        (c.Value == "Admin" || c.Value == "Editor"))));
        }
    }
}