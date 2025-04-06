using FlexCore.Caching.Core.Interfaces;
using Microsoft.Extensions.Options;
using System;

namespace FlexCore.Caching.Factory
{
    public class CacheFactoryOptions
    {
        public bool ValidateProvidersOnStartup { get; set; } = true;

        public void ValidateProviders(ICacheFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var providers = factory.GetRegisteredProviders().ToList();
            if (!providers.Any())
                throw new InvalidOperationException("Nessun provider registrato.");

            foreach (var provider in providers)
            {
                try
                {
                    factory.CreateCacheProvider(provider);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        $"Provider '{provider}' non valido. Errore: {ex.Message}", ex
                    );
                }
            }
        }
    }
}