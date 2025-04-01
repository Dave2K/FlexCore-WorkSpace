using FlexCore.Database.Interfaces;
using System;
using System.Collections.Generic;

namespace FlexCore.Database.Factory
{
    public class DatabaseProviderFactory : IDatabaseProviderFactory
    {
        private readonly Dictionary<string, Func<string, IDbConnectionFactory>> _providers = new();

        public void RegisterProvider(string name, Func<string, IDbConnectionFactory> providerFactory)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome provider non valido", nameof(name));

            _providers[name.ToUpperInvariant()] = providerFactory;
        }

        public IDbConnectionFactory CreateProvider(string providerName, string connectionString)
        {
            var key = providerName.ToUpperInvariant();

            if (_providers.TryGetValue(key, out var factory))
            {
                return factory(connectionString);
            }

            throw new NotSupportedException($"Provider '{providerName}' non supportato. Provider registrati: {string.Join(", ", _providers.Keys)}");
        }
    }
}