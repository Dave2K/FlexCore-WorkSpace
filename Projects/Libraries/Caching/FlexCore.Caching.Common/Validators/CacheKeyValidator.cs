using System;
using System.Text.RegularExpressions;

namespace FlexCore.Caching.Common.Validators
{
    /// <summary>
    /// Gestore centralizzato per la validazione delle chiavi di cache
    /// </summary>
    public static class CacheKeyValidator
    {
        private static readonly Regex _keyRegex = new(@"^[a-zA-Z0-9_-]{1,128}$", RegexOptions.Compiled);

        /// <summary>
        /// Esegue la validazione completa di una chiave
        /// </summary>
        /// <param name="key">Chiave da validare</param>
        /// <exception cref="ArgumentNullException">Chiave null</exception>
        /// <exception cref="ArgumentException">Formato chiave non valido</exception>
        public static void ThrowIfInvalid(string key)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key), "La chiave non può essere null");

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("La chiave non può essere vuota o contenere solo spazi", nameof(key));

            if (!_keyRegex.IsMatch(key))
            {
                throw new ArgumentException(
                    $"Formato chiave non valido: {key}. Caratteri permessi: A-Z, a-z, 0-9, -, _",
                    nameof(key)
                );
            }
        }
    }
}