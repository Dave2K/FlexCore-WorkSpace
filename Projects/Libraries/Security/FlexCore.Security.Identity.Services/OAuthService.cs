using FlexCore.Security.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Google.Apis.Auth;

namespace FlexCore.Security.Identity.Services
{
    public sealed class OAuthService
    {
        public async Task<ClaimsPrincipal> AuthenticateWithGoogleAsync(string token)
        {
            var payload = await ValidateGoogleToken(token);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, payload.Subject),
                new(ClaimTypes.Email, payload.Email),
                new(ClaimTypes.Name, payload.Name)
            };

            return new ClaimsPrincipal(new ClaimsIdentity(claims, "Google"));
        }

        private static async Task<GoogleTokenPayload> ValidateGoogleToken(string token)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(token);
            return new GoogleTokenPayload
            {
                Subject = payload.Subject,
                Email = payload.Email,
                Name = payload.Name
            };
        }
    }
}