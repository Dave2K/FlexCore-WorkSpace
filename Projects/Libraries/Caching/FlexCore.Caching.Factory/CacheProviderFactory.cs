using FlexCore.Caching.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlexCore.Caching.Factory
{
    /// <summary>
    /// Factory per la creazione di provider di cache
    /// </summary>
    public class CacheProviderFactory : ICacheFactory
    {
        private readonly Dictionary<string, Func<ICacheProvider>> _providers;

        /// <summary>
        /// Inizializza una nuova istanza del factory
        /// </summary>
        public CacheProviderFactory()
        {
            _providers = new Dictionary<string, Func<ICacheProvider>>(
                StringComparer.OrdinalIgnoreCase
            );
        }

        /// <inheritdoc/>
        public ICacheProvider CreateCacheProvider(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (!_providers.TryGetValue(name, out var creator))
            {
                throw new ArgumentException(
                    $"Provider '{name}' non registrato. Provider disponibili: {string.Join(", ", _providers.Keys)}",
                    nameof(name) // ✅ Aggiunto parametro name
                );
            }

            return creator();
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetRegisteredProviders() => _providers.Keys.ToList();

        /// <inheritdoc/>
        public void RegisterProvider(string name, Func<ICacheProvider> creator)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            _providers[name] = creator ?? throw new ArgumentNullException(nameof(creator));
        }
    }
}