namespace FlexCore.ORM.Core.Factory;

using FlexCore.ORM.Core.Interfaces;
using System;
using System.Collections.Generic;

/// <summary>
/// Factory per la creazione e registrazione dinamica dei provider ORM.
/// </summary>
public class OrmProviderFactory : IOrmProviderFactory
{
    private readonly Dictionary<string, Func<string, IOrmProvider>> _providers = new();

    /// <summary>
    /// Registra un provider ORM con un nome specifico.
    /// </summary>
    /// <param name="name">Nome del provider (es. "EFCore", "Dapper", "ADO").</param>
    /// <param name="providerFactory">Funzione che crea un'istanza del provider.</param>
    public void RegisterProvider(string name, Func<string, IOrmProvider> providerFactory)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Il nome del provider non può essere vuoto.", nameof(name));

        if (providerFactory == null)
            throw new ArgumentNullException(nameof(providerFactory), "La factory del provider non può essere nulla.");

        _providers[name] = providerFactory;
    }

    /// <summary>
    /// Crea un'istanza di un provider ORM in base al nome e alla stringa di connessione.
    /// </summary>
    /// <param name="providerName">Nome del provider da creare.</param>
    /// <param name="connectionString">Stringa di connessione al database.</param>
    /// <returns>Un'istanza del provider ORM.</returns>
    public IOrmProvider CreateProvider(string providerName, string connectionString)
    {
        if (_providers.TryGetValue(providerName, out var providerFactory))
        {
            return providerFactory(connectionString);
        }
        throw new NotSupportedException($"Provider '{providerName}' non supportato.");
    }
}