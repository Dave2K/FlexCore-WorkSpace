using System;
using System.Text.RegularExpressions;

namespace FlexCore.Caching.Common.Validators
{
    /// <summary>  
    /// Fornisce metodi per la validazione delle chiavi di cache.  
    /// </summary>  
    public static partial class CacheKeyValidator // ✅ Aggiunto "partial"  
    {
        private static readonly Regex _keyRegex = new Regex(
            pattern: @"^[a-zA-Z0-9_-]{1,128}$",
            options: RegexOptions.Compiled
        );

        /// <summary>  
        /// Verifica se una chiave rispetta il formato richiesto.  
        /// </summary>  
        public static void ValidateKey(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), "La chiave non può essere null.");

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("La chiave non può essere vuota o contenere solo spazi.", nameof(key));

            if (!_keyRegex.IsMatch(key))
            {
                throw new ArgumentException(
                    $"Formato chiave non valido: {key}. " +
                    "Caratteri consentiti: lettere, numeri, '-', '_'. Lunghezza: 1-128 caratteri.",
                    nameof(key)
                );
            }
        }
    }
}