namespace FlexCore.Caching.Core;

using FlexCore.Caching.Handlers;
using FlexCore.Caching.Interfaces;
using FlexCore.Caching.Validators;
using System;

/// <summary>
/// Classe base astratta per la gestione della cache.
/// </summary>
public abstract class BaseCacheManager : ICacheService
{
    /// <summary>
    /// Ottiene un valore dalla cache.
    /// </summary>
    /// <typeparam name="T">Tipo del valore da ottenere.</typeparam>
    /// <param name="key">Chiave del valore.</param>
    /// <returns>Il valore associato alla chiave.</returns>
    public abstract T Get<T>(string key);

    /// <summary>
    /// Imposta un valore nella cache.
    /// </summary>
    /// <typeparam name="T">Tipo del valore da impostare.</typeparam>
    /// <param name="key">Chiave del valore.</param>
    /// <param name="value">Valore da impostare.</param>
    /// <param name="expiration">Durata della cache.</param>
    public abstract void Set<T>(string key, T value, TimeSpan expiration);

    /// <summary>
    /// Rimuove un valore dalla cache.
    /// </summary>
    /// <param name="key">Chiave del valore da rimuovere.</param>
    public abstract void Remove(string key);

    /// <summary>
    /// Verifica se una chiave esiste nella cache.
    /// </summary>
    /// <param name="key">Chiave da verificare.</param>
    /// <returns>True se la chiave esiste, altrimenti false.</returns>
    public abstract bool Exists(string key);

    /// <summary>
    /// Valida la chiave della cache.
    /// </summary>
    /// <param name="key">Chiave da validare.</param>
    protected static void ValidateKey(string key) => CacheKeyValidator.ValidateKey(key);

    /// <summary>
    /// Gestisce le eccezioni durante le operazioni di cache.
    /// </summary>
    /// <param name="ex">Eccezione.</param>
    /// <param name="operation">Operazione che ha generato l'eccezione.</param>
    protected static void HandleException(Exception ex, string operation) => CacheExceptionHandler.HandleException(ex, operation);
}