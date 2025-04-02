using System;
using System.Collections.Generic;
using FlexCore.Caching.Core.Interfaces;

namespace FlexCore.Caching.Factory
{
    /// <summary>
    /// Factory per la creazione di provider di cache
    /// </summary>
    public class CacheProviderFactory : ICacheFactory
    {
        private readonly Dictionary<string, Func<ICacheProvider>> _providers;

        public CacheProviderFactory()
        {
            _providers = new Dictionary<string, Func<ICacheProvider>>(
                StringComparer.OrdinalIgnoreCase
            );
        }

        public ICacheProvider CreateCacheProvider(string name)
        {
            if (_providers.TryGetValue(name, out var provider))
            {
                return provider();
            }
            throw new ArgumentException($"Provider '{name}' non registrato");
        }

        public void RegisterProvider(string name, Func<ICacheProvider> creator)
        {
            _providers[name] = creator ?? throw new ArgumentNullException(nameof(creator));
        }
    }
}