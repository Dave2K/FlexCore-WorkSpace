namespace FlexCore.Caching.Interfaces;

/// <summary>
/// Interfaccia per la factory che crea istanze di provider di cache.
/// </summary>
public interface ICacheFactory
{
    /// <summary>
    /// Crea un'istanza di un provider di cache in base al nome.
    /// </summary>
    /// <param name="providerName">Nome del provider da creare.</param>
    /// <returns>Un'istanza del provider di cache.</returns>
    ICacheProvider CreateProvider(string providerName);
}