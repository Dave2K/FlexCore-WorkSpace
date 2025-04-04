using System;
using System.Text.RegularExpressions;

namespace FlexCore.Caching.Common.Validators
{
    /// <summary>
    /// Fornisce metodi per la validazione delle chiavi di cache
    /// </summary>
    public static partial class CacheKeyValidator
    {
        /// <summary>
        /// Regex precompilata per la validazione
        /// </summary>
        /// <remarks>
        /// Formato consentito: lettere, numeri, trattini e underscore (1-128 caratteri)
        /// </remarks>
        [GeneratedRegex(@"^[a-zA-Z0-9_-]{1,128}$", RegexOptions.Compiled)]
        private static partial Regex KeyRegex();

        /// <summary>
        /// Verifica la validità di una chiave
        /// </summary>
        /// <param name="key">Chiave da validare</param>
        /// <returns>True se valida, altrimenti False</returns>
        /// <exception cref="ArgumentNullException">Se key è null</exception>
        public static bool ValidateKey(string key)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            return KeyRegex().IsMatch(key);
        }
    }
}