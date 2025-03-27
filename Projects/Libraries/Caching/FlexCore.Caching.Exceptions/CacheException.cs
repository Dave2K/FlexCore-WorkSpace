namespace FlexCore.Caching.Exceptions;

using System;

/// <summary>
/// Eccezione personalizzata per gli errori di cache.
/// </summary>
public class CacheException : Exception
{
    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="CacheException"/>.
    /// </summary>
    /// <param name="message">Messaggio di errore.</param>
    public CacheException(string message) : base(message) { }

    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="CacheException"/>.
    /// </summary>
    /// <param name="message">Messaggio di errore.</param>
    /// <param name="innerException">Eccezione interna.</param>
    public CacheException(string message, Exception innerException) : base(message, innerException) { }
}