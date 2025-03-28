using FlexCore.Security.Identity.Models;
using System.Threading.Tasks;

namespace FlexCore.Security.Identity.Services
{
    public interface IGoogleTokenValidator
    {
        Task<GoogleTokenPayload> ValidateAsync(string token);
    }
}