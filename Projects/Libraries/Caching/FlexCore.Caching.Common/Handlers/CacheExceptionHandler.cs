namespace FlexCore.Caching.Common.Handlers;

using FlexCore.Caching.Common.Exceptions;
using System;

/// <summary>
/// Classe statica per la gestione delle eccezioni relative alla cache.
/// </summary>
public static class CacheExceptionHandler
{
    /// <summary>
    /// Gestisce le eccezioni durante le operazioni di cache.
    /// </summary>
    public static void HandleException(Exception ex, string operation)
    {
        throw ex switch
        {
            RedisCacheException => new CacheException($"Errore Redis durante {operation}", ex),
            MemoryCacheException => new CacheException($"Errore di memoria cache durante {operation}", ex),
            _ => new CacheException($"Errore durante l'operazione di cache: {operation}", ex)
        };
    }
}