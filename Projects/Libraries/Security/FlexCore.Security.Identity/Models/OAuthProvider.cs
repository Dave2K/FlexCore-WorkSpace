namespace FlexCore.Security.Identity.Models
{
    public class OAuthProvider
    {
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
        public required string RedirectUri { get; set; }
    }
}