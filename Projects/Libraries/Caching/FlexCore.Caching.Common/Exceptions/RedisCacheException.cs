namespace FlexCore.Caching.Common.Exceptions;

using System;

/// <summary>
/// Eccezione personalizzata per gli errori di cache Redis.
/// </summary>
public class RedisCacheException : CacheException
{
    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="RedisCacheException"/>.
    /// </summary>
    /// <param name="message">Messaggio di errore.</param>
    public RedisCacheException(string message) : base(message) { }

    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="RedisCacheException"/>.
    /// </summary>
    /// <param name="message">Messaggio di errore.</param>
    /// <param name="innerException">Eccezione interna.</param>
    public RedisCacheException(string message, Exception innerException) : base(message, innerException) { }
}