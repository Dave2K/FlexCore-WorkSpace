using Dapper;
using System.Data;

namespace FlexCore.Northwind;

/// <summary>
/// Implementazione di <see cref="IRepository{T}"/> che utilizza Dapper per le operazioni CRUD.
/// </summary>
/// <typeparam name="T">Tipo dell'entità da gestire.</typeparam>
public class DapperRepository<T> : IRepository<T> where T : class
{
    private readonly IDbConnection _dbConnection;

    /// <summary>
    /// Costruttore per inizializzare la connessione a Dapper.
    /// </summary>
    /// <param name="dbConnection">Connessione al database (ad esempio, SqlConnection o NpgsqlConnection).</param>
    public DapperRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    /// <summary>
    /// Aggiunge una nuova entità al database.
    /// </summary>
    /// <param name="entity">Entità da aggiungere.</param>
    public async Task AddAsync(T entity)
    {
        var query = $"INSERT INTO {typeof(T).Name} ({string.Join(", ", GetColumns(entity))}) VALUES (@{string.Join(", @", GetColumns(entity))})";
        await _dbConnection.ExecuteAsync(query, entity);
    }

    /// <summary>
    /// Ottiene tutte le entità dal database.
    /// </summary>
    /// <returns>Lista di entità di tipo <typeparamref name="T"/>.</returns>
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var query = $"SELECT * FROM {typeof(T).Name}";
        return await _dbConnection.QueryAsync<T>(query);
    }

    /// <summary>
    /// Ottiene un'entità per ID.
    /// </summary>
    /// <param name="id">ID dell'entità da cercare.</param>
    /// <returns>Entità trovata.</returns>
    public async Task<T?> GetByIdAsync(object id)
    {
        var query = $"SELECT * FROM {typeof(T).Name} WHERE Id = @Id";
        return await _dbConnection.QueryFirstOrDefaultAsync<T>(query, new { Id = id });
    }

    /// <summary>
    /// Modifica un'entità nel database.
    /// </summary>
    /// <param name="entity">Entità da modificare.</param>
    public async Task UpdateAsync(T entity)
    {
        var query = $"UPDATE {typeof(T).Name} SET {string.Join(", ", GetColumns(entity).Select(c => $"{c} = @{c}"))} WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(query, entity);
    }

    /// <summary>
    /// Elimina un'entità dal database.
    /// </summary>
    /// <param name="id">ID dell'entità da eliminare.</param>
    public async Task DeleteAsync(object id)
    {
        var query = $"DELETE FROM {typeof(T).Name} WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(query, new { Id = id });
    }

    /// <summary>
    /// Ottiene i nomi delle colonne di un'entità.
    /// </summary>
    /// <param name="entity">Entità da cui estrarre i nomi delle colonne.</param>
    /// <returns>Lista di nomi delle colonne.</returns>
    private IEnumerable<string> GetColumns(T entity)
    {
        return entity.GetType().GetProperties().Select(p => p.Name);
    }
}
