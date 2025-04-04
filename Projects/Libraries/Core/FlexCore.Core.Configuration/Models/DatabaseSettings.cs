namespace FlexCore.Core.Configuration.Models;

/// <summary>
/// Configurazione globale per la gestione dei database nell'applicazione
/// </summary>
public class DatabaseSettings
{
    /// <summary>
    /// Provider di database predefinito utilizzato dall'applicazione
    /// </summary>
    /// <example>SQLServer, SQLite, MariaDB</example>
    public required string DefaultProvider { get; set; }

    /// <summary>
    /// Elenco dei provider di database supportati dall'applicazione
    /// </summary>
    /// <example>["SQLServer", "SQLite", "MariaDB"]</example>
    public List<string> Providers { get; set; } = new List<string>();

    /// <summary>
    /// Configurazioni specifiche per Microsoft SQL Server
    /// </summary>
    public required SQLServerSettings SQLServer { get; set; }

    /// <summary>
    /// Configurazioni specifiche per SQLite
    /// </summary>
    public required SQLiteSettings SQLite { get; set; }

    /// <summary>
    /// Configurazioni specifiche per MariaDB
    /// </summary>
    public required MariaDBSettings MariaDB { get; set; }
}