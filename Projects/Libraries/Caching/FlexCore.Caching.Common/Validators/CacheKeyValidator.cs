namespace FlexCore.Caching.Common.Validators;

using System;

/// <summary>
/// Classe statica per la validazione delle chiavi di cache.
/// </summary>
public static class CacheKeyValidator
{
    /// <summary>
    /// Verifica che la chiave della cache non sia nulla, vuota o composta solo da spazi bianchi.
    /// </summary>
    /// <param name="key">Chiave da validare.</param>
    public static void ValidateKey(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("La chiave non può essere nulla o composta solo da spazi vuoti.", nameof(key));
    }
}
