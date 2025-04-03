using Dapper;
using System.Data;

namespace FlexCore.Northwind;

/// <summary>
/// Implementazione di <see cref="DatabaseContext"/> che utilizza Dapper per l'accesso ai dati.
/// </summary>
public class DapperDatabaseContext : DatabaseContext
{
    private readonly IDbConnection _dbConnection;

    /// <summary>
    /// Costruttore per inizializzare la connessione a Dapper.
    /// </summary>
    /// <param name="dbConnection">Connessione al database (ad esempio, SqlConnection o NpgsqlConnection).</param>
    public DapperDatabaseContext(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    /// <summary>
    /// Esegue una query SQL e restituisce i risultati come una lista di oggetti.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entità di destinazione.</typeparam>
    /// <param name="query">Query SQL da eseguire.</param>
    /// <param name="parameters">Parametri opzionali per la query.</param>
    /// <returns>Lista di oggetti di tipo <typeparamref name="T"/>.</returns>
    public override async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query, object? parameters = null)
    {
        return await _dbConnection.QueryAsync<T>(query, parameters);
    }

    /// <summary>
    /// Esegue un comando SQL (INSERT, UPDATE, DELETE).
    /// </summary>
    /// <param name="query">Comando SQL da eseguire.</param>
    /// <param name="parameters">Parametri opzionali per il comando.</param>
    /// <returns>Numero di righe interessate.</returns>
    public override async Task<int> ExecuteCommandAsync(string query, object? parameters = null)
    {
        return await _dbConnection.ExecuteAsync(query, parameters);
    }
}
