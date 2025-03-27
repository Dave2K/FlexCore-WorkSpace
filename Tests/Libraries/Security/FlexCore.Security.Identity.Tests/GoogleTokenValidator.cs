using Google.Apis.Auth;
using FlexCore.Security.Identity.Models;

namespace FlexCore.Security.Identity.Services
{
    public class GoogleTokenValidator : IGoogleTokenValidator
    {
        public async Task<GoogleTokenPayload> ValidateAsync(string token)
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