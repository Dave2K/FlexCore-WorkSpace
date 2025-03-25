namespace FlexCore.Caching.Interfaces;

using System;

/// <summary>
/// Interfaccia per i servizi di cache.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Ottiene un valore dalla cache.
    /// </summary>
    /// <typeparam name="T">Tipo del valore da ottenere.</typeparam>
    /// <param name="key">Chiave del valore.</param>
    /// <returns>Il valore associato alla chiave.</returns>
    T Get<T>(string key);

    /// <summary>
    /// Imposta un valore nella cache.
    /// </summary>
    /// <typeparam name="T">Tipo del valore da impostare.</typeparam>
    /// <param name="key">Chiave del valore.</param>
    /// <param name="value">Valore da impostare.</param>
    /// <param name="expiration">Durata della cache.</param>
    void Set<T>(string key, T value, TimeSpan expiration);

    /// <summary>
    /// Rimuove un valore dalla cache.
    /// </summary>
    /// <param name="key">Chiave del valore da rimuovere.</param>
    void Remove(string key);

    /// <summary>
    /// Verifica se una chiave esiste nella cache.
    /// </summary>
    /// <param name="key">Chiave da verificare.</param>
    /// <returns>True se la chiave esiste, altrimenti false.</returns>
    bool Exists(string key);
}