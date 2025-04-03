using Microsoft.EntityFrameworkCore;

namespace FlexCore.Northwind;

/// <summary>
/// Implementazione di <see cref="DatabaseContext"/> che utilizza Entity Framework Core per l'accesso ai dati.
/// </summary>
public class EfDatabaseContext : DatabaseContext
{
    private readonly DbContext _dbContext;

    /// <summary>
    /// Costruttore per inizializzare il contesto di Entity Framework.
    /// </summary>
    /// <param name="dbContext">Istanza di DbContext per EF Core.</param>
    public EfDatabaseContext(DbContext dbContext)
    {
        _dbContext = dbContext;
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
        return await _dbContext.Set<T>().FromSqlRaw(query, parameters).ToListAsync();
    }

    /// <summary>
    /// Esegue un comando SQL (INSERT, UPDATE, DELETE).
    /// </summary>
    /// <param name="query">Comando SQL da eseguire.</param>
