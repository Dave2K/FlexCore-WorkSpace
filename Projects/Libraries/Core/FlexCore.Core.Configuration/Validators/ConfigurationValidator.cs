namespace FlexCore.Core.Configuration.Validators;

using System;

/// <summary>
/// Classe statica per la validazione degli input di configurazione.
/// </summary>
public static class ConfigurationValidator
{
    /// <summary>
    /// Verifica che la chiave di configurazione non sia nulla o vuota.
    /// </summary>
    /// <param name="key">La chiave da validare.</param>
    /// <exception cref="ArgumentNullException">Se la chiave è null</exception>
    /// <exception cref="ArgumentException">Se la chiave è vuota o spazi bianchi</exception>
    public static void ValidateKey(string key)
    {
        if (key is null)
            throw new ArgumentNullException(nameof(key), "La chiave non può essere nulla.");

        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("La chiave non può essere vuota o contenere solo spazi.", nameof(key));
    }
}