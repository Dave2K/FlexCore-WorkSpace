namespace FlexCore.Core.Configuration.Models;

/// <summary>
/// Configurazioni globali per la gestione dei database.
/// </summary>
public class DatabaseSettings
{
    /// <summary>
    /// Provider di database predefinito (es. "SQLServer").
    /// </summary>
    public required string DefaultProvider { get; set; }

    /// <summary>
    /// Elenco dei provider supportati.
    /// </summary>
    public List<string> Providers { get; set; } = new List<string>();

    /// <summary>
    /// Impostazioni specifiche per Microsoft SQL Server.
    /// </summary>
    public required SQLServerSettings SQLServer { get; set; }

    /// <summary>
    /// Impostazioni specifiche per SQLite.
    /// </summary>
    public required SQLiteSettings SQLite { get; set; }

    /// <summary>
    /// Impostazioni specifiche per MariaDB.
    /// </summary>
    public required MariaDBSettings MariaDB { get; set; }
}