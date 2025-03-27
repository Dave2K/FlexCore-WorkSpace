using System.Security.Claims;
using System.Threading.Tasks;

namespace FlexCore.Security.Identity.Services
{
    public sealed class OAuthService
    {
        private readonly IGoogleTokenValidator _tokenValidator;

        public OAuthService(IGoogleTokenValidator tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }

        public async Task<ClaimsPrincipal> AuthenticateWithGoogleAsync(string token)
        {
            var payload = await _tokenValidator.ValidateAsync(token);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, payload.Subject),
                new Claim(ClaimTypes.Email, payload.Email),
                new Claim(ClaimTypes.Name, payload.Name)
            };

            return new ClaimsPrincipal(new ClaimsIdentity(claims, "Google"));
        }
    }
}