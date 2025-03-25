namespace FlexCore.Core.Configuration.Models;

using System;
using System.Collections.Generic;

/// <summary>
/// Impostazioni del database.
/// </summary>
public class DatabaseSettings
{
    /// <summary>
    /// Provider predefinito.
    /// </summary>
    public required string DefaultProvider { get; set; }

    /// <summary>
    /// Lista dei provider supportati.
    /// </summary>
    public List<string> Providers { get; set; } = new List<string>();

    /// <summary>
    /// Impostazioni specifiche per SQL Server.
    /// </summary>
    public required SQLServerSettings SQLServer { get; set; }

    /// <summary>
    /// Impostazioni specifiche per SQLite.
    /// </summary>
    public required SQLiteSettings SQLite { get; set; }
}

/// <summary>
/// Impostazioni specifiche per SQL Server.
/// </summary>
public class SQLServerSettings
{
    /// <summary>
    /// Abilita i tentativi di riconnessione in caso di errore.
    /// </summary>
    public required bool EnableRetryOnFailure { get; set; }

    /// <summary>
    /// Numero massimo di tentativi.
    /// </summary>
    public required int MaxRetryCount { get; set; }

    /// <summary>
    /// Tempo massimo di attesa tra i tentativi.
    /// </summary>
    public required TimeSpan MaxRetryDelay { get; set; }
}

/// <summary>
/// Impostazioni specifiche per SQLite.
/// </summary>
public class SQLiteSettings
{
    /// <summary>
    /// Dimensione della cache SQLite.
    /// </summary>
    public required int CacheSize { get; set; }

    /// <summary>
    /// Modalità di sincronizzazione (Off, Normal, Full).
    /// </summary>
    public required string Synchronous { get; set; }
}