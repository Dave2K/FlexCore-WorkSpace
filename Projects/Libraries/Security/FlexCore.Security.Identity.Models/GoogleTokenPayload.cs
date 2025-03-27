namespace FlexCore.Security.Identity.Models
{
    public class GoogleTokenPayload
    {
        public required string Subject { get; set; }
        public required string Email { get; set; }
        public required string Name { get; set; }
    }
}