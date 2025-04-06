using FlexCore.Caching.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace FlexCore.Caching.Factory
{
    /// <summary>
    /// Implementazione concreta di <see cref="ICacheFactory"/> per la gestione di provider di cache
    /// </summary>
    /// <remarks>
    /// Utilizza un dizionario case-insensitive per la registrazione dei provider.
    /// Supporta la registrazione dinamica e la sovrascrittura dei provider.
    /// </remarks>
    public class CacheProviderFactory : ICacheFactory
    {
        private readonly Dictionary<string, Func<ICacheProvider>> _providers;

        /// <summary>
        /// Inizializza una nuova istanza del factory
        /// </summary>
        public CacheProviderFactory()
        {
            _providers = new Dictionary<string, Func<ICacheProvider>>(
                StringComparer.OrdinalIgnoreCase);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Se <paramref name="name"/> è null o vuoto</exception>
        /// <exception cref="ArgumentException">Se il provider non è registrato</exception>
        public ICacheProvider CreateCacheProvider(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Il nome del provider è obbligatorio");

            if (!_providers.TryGetValue(name, out var provider))
            {
                throw new ArgumentException(
                    $"Provider '{name}' non registrato. Provider disponibili: {string.Join(", ", _providers.Keys)}",
                    nameof(name));
            }

            return provider();
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">
        /// Se <paramref name="name"/> o <paramref name="creator"/> sono null/vuoti
        /// </exception>
        public void RegisterProvider(string name, Func<ICacheProvider> creator)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Il nome del provider è obbligatorio");

            _providers[name] = creator ?? throw new ArgumentNullException(
                nameof(creator),
                "La factory method del provider è obbligatoria");
        }
    }
}