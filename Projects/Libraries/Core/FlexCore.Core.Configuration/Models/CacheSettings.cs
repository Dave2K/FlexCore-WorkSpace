namespace FlexCore.Core.Configuration.Models;

using System;
using System.Collections.Generic;

/// <summary>
/// Impostazioni della cache.
/// </summary>
public class CacheSettings
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
    /// Impostazioni specifiche per la cache in memoria.
    /// </summary>
    public required MemoryCacheSettings MemoryCache { get; set; }

    /// <summary>
    /// Impostazioni specifiche per la cache Redis.
    /// </summary>
    public required RedisSettings Redis { get; set; }
}

/// <summary>
/// Impostazioni specifiche per la cache in memoria.
/// </summary>
public class MemoryCacheSettings
{
    /// <summary>
    /// Limite di dimensione della cache in memoria.
    /// </summary>
    public required int SizeLimit { get; set; }

    /// <summary>
    /// Percentuale di compattazione della cache.
    /// </summary>
    public required double CompactionPercentage { get; set; }

    /// <summary>
    /// Frequenza di scansione per le scadenze.
    /// </summary>
    public TimeSpan ExpirationScanFrequency { get; set; }
}

/// <summary>
/// Impostazioni specifiche per la cache Redis.
/// </summary>
public class RedisSettings
{
    /// <summary>
    /// Stringa di connessione Redis.
    /// </summary>
    public required string ConnectionString { get; set; }

    /// <summary>
    /// Nome dell'istanza Redis.
    /// </summary>
    public required string InstanceName { get; set; }

    /// <summary>
    /// Database predefinito.
    /// </summary>
    public required int DefaultDatabase { get; set; }

    /// <summary>
    /// Se interrompere la connessione in caso di errore.
    /// </summary>
    public required bool AbortOnConnectFail { get; set; }

    /// <summary>
    /// Timeout di connessione in millisecondi.
    /// </summary>
    public required int ConnectTimeout { get; set; }

    /// <summary>
    /// Timeout di sincronizzazione in millisecondi.
    /// </summary>
    public required int SyncTimeout { get; set; }
}