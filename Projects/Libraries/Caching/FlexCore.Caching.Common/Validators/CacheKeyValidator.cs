using System;
using System.Text.RegularExpressions;

namespace FlexCore.Caching.Common.Validators
{
    public static class CacheKeyValidator
    {
        private static readonly Regex _keyRegex = new Regex(@"^[a-zA-Z0-9_-]{1,128}$", RegexOptions.Compiled);

        public static void ThrowIfInvalid(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), "La chiave non può essere null.");

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("La chiave non può essere vuota o contenere solo spazi.", nameof(key));

            if (!_keyRegex.IsMatch(key))
                throw new ArgumentException(
                    $"Formato chiave non valido: {key}. Caratteri consentiti: lettere, numeri, '-', '_'. Lunghezza: 1-128 caratteri.",
                    nameof(key)
                );
        }
    }
}