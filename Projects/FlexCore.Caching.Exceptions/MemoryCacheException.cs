namespace FlexCore.Caching.Exceptions;

using System;

/// <summary>
/// Eccezione personalizzata per gli errori di cache in memoria.
/// </summary>
public class MemoryCacheException : CacheException
{
    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="MemoryCacheException"/>.
    /// </summary>
    /// <param name="message">Messaggio di errore.</param>
    public MemoryCacheException(string message) : base(message) { }

    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="MemoryCacheException"/>.
    /// </summary>
    /// <param name="message">Messaggio di errore.</param>
    /// <param name="innerException">Eccezione interna.</param>
    public MemoryCacheException(string message, Exception innerException) : base(message, innerException) { }
}