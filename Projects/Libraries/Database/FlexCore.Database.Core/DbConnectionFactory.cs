namespace FlexCore.Database.Core;

using System.Data;
using System.Threading.Tasks;
using FlexCore.Database.Interfaces;
using Microsoft.Data.SqlClient;

/// <summary>
/// Classe astratta base per la creazione di connessioni a database.
/// Fornisce un'implementazione generica per tutti i provider.
/// </summary>
public abstract class DbConnectionFactory : IDbConnectionFactory
{
    /// <summary>
    /// Stringa di connessione al database.
    /// </summary>
    protected readonly string _connectionString;

    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="DbConnectionFactory"/>.
    /// </summary>
    /// <param name="connectionString">Stringa di connessione al database. Non può essere null.</param>
    /// <exception cref="ArgumentNullException">Se <paramref name="connectionString"/> è null.</exception>
    protected DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    /// <summary>
    /// Crea e restituisce una connessione aperta al database.
    /// </summary>
    /// <returns>Istanza di <see cref="IDbConnection"/> aperta.</returns>
    public abstract IDbConnection CreateConnection();

    /// <summary>
    /// Crea e restituisce una connessione aperta al database in modo asincrono (implementazione predefinita).
    /// </summary>
    /// <returns>Task che restituisce la connessione aperta.</returns>
    public virtual async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = CreateConnection();
        await ((SqlConnection)connection).OpenAsync();
        return connection;
    }
}