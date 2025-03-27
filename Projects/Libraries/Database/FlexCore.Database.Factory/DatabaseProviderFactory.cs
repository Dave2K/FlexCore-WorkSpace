namespace FlexCore.Database.Factory;

using FlexCore.Database.Interfaces;
using System;
using System.Collections.Generic;

/// <summary>
/// Factory per la creazione dinamica di provider di database basati su nome e stringa di connessione.
/// </summary>
public class DatabaseProviderFactory : IDatabaseProviderFactory
{
    private readonly Dictionary<string, Func<string, IDbConnectionFactory>> _providers = new();

    /// <summary>
    /// Registra un nuovo provider di database.
    /// </summary>
    /// <param name="name">Nome identificativo del provider (es. "SQLServer", "SQLite").</param>
    /// <param name="providerFactory">Factory che crea un'istanza del provider.</param>
    /// <exception cref="ArgumentException">Se <paramref name="name"/> è vuoto o nullo.</exception>
    /// <exception cref="ArgumentNullException">Se <paramref name="providerFactory"/> è nullo.</exception>
    public void RegisterProvider(string name, Func<string, IDbConnectionFactory> providerFactory)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Il nome del provider non può essere vuoto.", nameof(name));

        _providers[name] = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
    }

    /// <summary>
    /// Crea un'istanza del provider specificato.
    /// </summary>
    /// <param name="providerName">Nome del provider registrato.</param>
    /// <param name="connectionString">Stringa di connessione da utilizzare.</param>
    /// <returns>Istanza del provider di database.</returns>
    /// <exception cref="NotSupportedException">Se il provider non è registrato.</exception>
    public IDbConnectionFactory CreateProvider(string providerName, string connectionString)
    {
        if (_providers.TryGetValue(providerName, out var factory))
        {
            return factory(connectionString);
        }
        throw new NotSupportedException($"Provider '{providerName}' non supportato.");
    }
}