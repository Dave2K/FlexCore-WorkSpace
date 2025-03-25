namespace FlexCore.Caching.Memory;

using FlexCore.Caching.Core;
using FlexCore.Caching.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;

/// <summary>
/// Provider di cache in memoria.
/// </summary>
public class MemoryCacheProvider : BaseCacheManager, ICacheProvider
{
    private readonly IMemoryCache _cache;

    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="MemoryCacheProvider"/>.
    /// </summary>
    /// <param name="cache">Istanza di <see cref="IMemoryCache"/>.</param>
    public MemoryCacheProvider(IMemoryCache cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    /// <summary>
    /// Ottiene un valore dalla cache.
    /// </summary>
    /// <typeparam name="T">Tipo del valore da ottenere.</typeparam>
    /// <param name="key">Chiave del valore.</param>
    /// <returns>Il valore associato alla chiave.</returns>
    public override T Get<T>(string key)
    {
        ValidateKey(key);
        return _cache.Get<T>(key) ?? default!;
    }

    /// <summary>
    /// Imposta un valore nella cache.
    /// </summary>
    /// <typeparam name="T">Tipo del valore da impostare.</typeparam>
    /// <param name="key">Chiave del valore.</param>
    /// <param name="value">Valore da impostare.</param>
    /// <param name="expiration">Durata della cache.</param>
    public override void Set<T>(string key, T value, TimeSpan expiration)
    {
        ValidateKey(key);
        _cache.Set(key, value, expiration);
        if (!_cache.TryGetValue(key, out _))
            throw new Exception($"Set() failed for key: {key}");
    }

    /// <summary>
    /// Rimuove un valore dalla cache.
    /// </summary>
    /// <param name="key">Chiave del valore da rimuovere.</param>
    public override void Remove(string key)
    {
        ValidateKey(key);
        _cache.Remove(key);
    }

    /// <summary>
    /// Verifica se una chiave esiste nella cache.
    /// </summary>
    /// <param name="key">Chiave da verificare.</param>
    /// <returns>True se la chiave esiste, altrimenti false.</returns>
    public override bool Exists(string key)
    {
        ValidateKey(key);
        return _cache.TryGetValue(key, out _);
    }

    /// <summary>
    /// Svuota completamente la cache.
    /// </summary>
    public void ClearAll()
    {
        if (_cache is MemoryCache memoryCache)
        {
            memoryCache.Compact(1.0); // Svuota completamente la cache
        }
    }
}