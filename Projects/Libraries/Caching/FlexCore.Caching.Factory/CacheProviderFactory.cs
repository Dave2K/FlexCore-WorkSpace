using FlexCore.Caching.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlexCore.Caching.Factory
{
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
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (!_providers.TryGetValue(name, out var creator))
            {
                throw new ArgumentException(
                    $"Provider '{name}' non registrato. Provider disponibili: {string.Join(", ", _providers.Keys)}"
                );
            }

            return creator();
        }

        public IEnumerable<string> GetRegisteredProviders()
        {
            return _providers.Keys.ToList();
        }

        public void RegisterProvider(string name, Func<ICacheProvider> creator)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            _providers[name] = creator ?? throw new ArgumentNullException(nameof(creator));
        }
    }
}