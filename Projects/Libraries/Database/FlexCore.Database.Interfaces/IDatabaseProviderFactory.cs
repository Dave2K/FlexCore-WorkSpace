namespace FlexCore.Database.Interfaces;

/// <summary>
/// Interfaccia per la factory che crea istanze di provider di database.
/// </summary>
public interface IDatabaseProviderFactory
{
    /// <summary>
    /// Crea un'istanza di un provider di database in base al nome e alla stringa di connessione.
    /// </summary>
    /// <param name="providerName">Nome del provider.</param>
    /// <param name="connectionString">Stringa di connessione al database.</param>
    /// <returns>Un'istanza del provider di database.</returns>
    IDbConnectionFactory CreateProvider(string providerName, string connectionString);
}