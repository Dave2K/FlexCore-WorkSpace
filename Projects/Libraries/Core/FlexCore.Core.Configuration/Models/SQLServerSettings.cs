namespace FlexCore.Core.Configuration.Models;

/// <summary>
/// Impostazioni specifiche per la connessione a Microsoft SQL Server
/// </summary>
public class SQLServerSettings
{
    /// <summary>
    /// Stringa di connessione al database
    /// </summary>
    /// <example>Server=.;Database=MyDB;User Id=user;Password=password;</example>
    public required string ConnectionString { get; set; }

    /// <summary>
    /// Abilita il tentativo automatico di riconnessione in caso di errori temporanei
    /// </summary>
    public bool EnableRetryOnFailure { get; set; }

    /// <summary>
    /// Numero massimo di tentativi di riconnessione
    /// </summary>
    public int MaxRetryCount { get; set; }

    /// <summary>
    /// Intervallo massimo tra i tentativi di riconnessione
    /// </summary>
    /// <example>00:00:30 (30 secondi)</example>
    public required TimeSpan MaxRetryDelay { get; set; }
}