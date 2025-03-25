namespace FlexCore.Caching.Redis;

using FlexCore.Caching.Core;
using FlexCore.Caching.Interfaces;
using StackExchange.Redis;
using System;
using System.Text.Json;

/// <summary>
/// Provider di cache Redis.
/// </summary>
public class RedisCacheProvider : BaseCacheManager, ICacheProvider
{
    private readonly IDatabase _database;

    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="RedisCacheProvider"/>.
    /// </summary>
    /// <param name="connection">Istanza di <see cref="IConnectionMultiplexer"/>.</param>
    public RedisCacheProvider(IConnectionMultiplexer connection)
    {
        _database = connection.GetDatabase();
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
        var value = _database.StringGet(key);

        if (value.IsNullOrEmpty)
            return default!; // Restituisce il valore predefinito di T

        return JsonSerializer.Deserialize<T>(value!) ?? default!;
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
        var json = JsonSerializer.Serialize(value);
        _database.StringSet(key, json, expiration);
    }

    /// <summary>
    /// Rimuove un valore dalla cache.
    /// </summary>
    /// <param name="key">Chiave del valore da rimuovere.</param>
    public override void Remove(string key)
    {
        ValidateKey(key);
        _database.KeyDelete(key);
    }

    /// <summary>
    /// Verifica se una chiave esiste nella cache.
    /// </summary>
    /// <param name="key">Chiave da verificare.</param>
    /// <returns>True se la chiave esiste, altrimenti false.</returns>
    public override bool Exists(string key)
    {
        ValidateKey(key);
        return _database.KeyExists(key);
    }
}