namespace FlexCore.Core.Configuration.Models;

/// <summary>
/// Impostazioni specifiche per Microsoft SQL Server.
/// </summary>
public class SQLServerSettings
{
    /// <summary>
    /// Stringa di connessione per SQL Server.
    /// </summary>
    public required string ConnectionString { get; set; }

    /// <summary>
    /// Abilita il tentativo di riconnessione automatica in caso di errori temporanei.
    /// </summary>
    public bool EnableRetryOnFailure { get; set; }

    /// <summary>
    /// Numero massimo di tentativi di riconnessione.
    /// </summary>
    public int MaxRetryCount { get; set; }

    /// <summary>
    /// Tempo massimo di attesa tra i tentativi di riconnessione (formato TimeSpan).
    /// </summary>
    public required TimeSpan MaxRetryDelay { get; set; }
}