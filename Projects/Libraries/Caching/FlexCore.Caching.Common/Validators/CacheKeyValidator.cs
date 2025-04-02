using System.Text.RegularExpressions;

namespace FlexCore.Caching.Common.Validators
{
    /// <summary>
    /// Fornisce metodi statici per la validazione delle chiavi della cache.
    /// </summary>
    public static partial class CacheKeyValidator
    {
        [GeneratedRegex("^[a-zA-Z0-9_-]+$", RegexOptions.Compiled)]
        private static partial Regex KeyRegex();

        /// <summary>
        /// Verifica se una chiave è valida per l'uso nella cache.
        /// </summary>
        /// <param name="key">La chiave da validare.</param>
        /// <returns>True se la chiave è valida, altrimenti False.</returns>
        public static bool ValidateKey(string key)
        {
            return KeyRegex().IsMatch(key);
        }
    }
}