namespace FlexCore.Core.Configuration.Models;

/// <summary>
/// Impostazioni specifiche per MariaDB.
/// </summary>
public class MariaDBSettings
{
    /// <summary>
    /// Stringa di connessione per MariaDB (es. "Server=localhost;Port=3306;Database=FlexDB;User=root;Password=mariaPassword;").
    /// </summary>
    public required string ConnectionString { get; set; }

    /// <summary>
    /// Abilita il connection pooling per migliorare le prestazioni.
    /// </summary>
    public required bool Pooling { get; set; }
}