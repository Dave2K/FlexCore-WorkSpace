namespace FlexCore.Core.Configuration.Models;

/// <summary>
/// Impostazioni specifiche per SQLite.
/// </summary>
public class SQLiteSettings
{
    /// <summary>
    /// Stringa di connessione per SQLite (es. "Data Source=./database.db;Version=3;").
    /// </summary>
    public required string ConnectionString { get; set; }

    /// <summary>
    /// Modalità del journaling del database (es. "WAL", "DELETE", "TRUNCATE").
    /// </summary>
    public required string JournalMode { get; set; }

    /// <summary>
    /// Dimensione della cache in pagine SQLite (valore predefinito: -2000 per 2 GB).
    /// </summary>
    public int CacheSize { get; set; } = -2000;

    /// <summary>
    /// Modalità di sincronizzazione dei dati su disco (es. "Normal", "Full", "Off").
    /// </summary>
    public required string Synchronous { get; set; }
}