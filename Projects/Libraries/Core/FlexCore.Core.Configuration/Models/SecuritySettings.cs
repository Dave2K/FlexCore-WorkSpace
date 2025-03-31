namespace FlexCore.Core.Configuration.Models;

/// <summary>
/// Impostazioni di sicurezza (JWT e OAuth).
/// </summary>
public class SecuritySettings
{
    /// <summary>
    /// Provider di sicurezza predefinito.
    /// </summary>
    public required string DefaultProvider { get; set; }

    /// <summary>
    /// Lista dei provider supportati.
    /// </summary>
    public List<string> Providers { get; set; } = new List<string>();

    /// <summary>
    /// Impostazioni JWT.
    /// </summary>
    public required JwtSettings JWT { get; set; }

    /// <summary>
    /// Impostazioni OAuth.
    /// </summary>
    public required OAuthSettings OAuth { get; set; }
}

/// <summary>
/// Impostazioni JWT.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Chiave segreta per la firma dei token.
    /// </summary>
    public required string SecretKey { get; set; }

    /// <summary>
    /// Emittente del token.
    /// </summary>
    public required string Issuer { get; set; }

    /// <summary>
    /// Destinatario del token.
    /// </summary>
    public required string Audience { get; set; }

    /// <summary>
    /// Durata del token in minuti.
    /// </summary>
    public int ExpiryMinutes { get; set; } = 120;
}

/// <summary>
/// Impostazioni OAuth per provider esterni.
/// </summary>
public class OAuthSettings
{
    /// <summary>
    /// Impostazioni per Google OAuth.
    /// </summary>
    public required GoogleSettings Google { get; set; }

    /// <summary>
    /// Impostazioni per Facebook OAuth.
    /// </summary>
    public required FacebookSettings Facebook { get; set; }
}

/// <summary>
/// Impostazioni specifiche per Google OAuth.
/// </summary>
public class GoogleSettings
{
    /// <summary>
    /// Client ID per l'autenticazione Google.
    /// </summary>
    public required string ClientId { get; set; }

    /// <summary>
    /// Client Secret per l'autenticazione Google.
    /// </summary>
    public required string ClientSecret { get; set; }
}

/// <summary>
/// Impostazioni specifiche per Facebook OAuth.
/// </summary>
public class FacebookSettings
{
    /// <summary>
    /// App ID per l'autenticazione Facebook.
    /// </summary>
    public required string ClientId { get; set; }

    /// <summary>
    /// App Secret per l'autenticazione Facebook.
    /// </summary>
    public required string ClientSecret { get; set; }
}