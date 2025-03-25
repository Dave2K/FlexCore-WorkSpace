namespace FlexCore.Core.Configuration.Models;

/// <summary>
/// Impostazioni delle stringhe di connessione.
/// </summary>
public class ConnectionStringsSettings
{
    /// <summary>
    /// Stringa di connessione predefinita.
    /// </summary>
    public required string DefaultDatabase { get; set; }

    /// <summary>
    /// Stringa di connessione per SQLite.
    /// </summary>
    public required string SQLiteDatabase { get; set; }

    /// <summary>
    /// Stringa di connessione per Redis.
    /// </summary>
    public required string Redis { get; set; }
}