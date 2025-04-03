using Microsoft.EntityFrameworkCore;

namespace FlexCore.Northwind;

/// <summary>
/// Implementazione di <see cref="IRepository{T}"/> che utilizza Entity Framework Core per le operazioni CRUD.
/// </summary>
/// <typeparam name="T">Tipo dell'entità da gestire.</typeparam>
public class EfRepository<T> : IRepository<T> where T : class
{
    private readonly DbContext _dbContext;

    /// <summary>
    /// Costruttore per inizializzare il contesto di Entity Framework.
    /// </summary>
    /// <param name="dbContext">Istanza di DbContext per EF Core.</param>
    public EfRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Aggiunge una nuova entità al database.
    /// </summary>
    /// <param name="entity">Entità da aggiungere.</param>
    public async Task AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Ottiene tutte le entità dal database.
    /// </summary>
    /// <returns>Lista di entità di tipo <typeparamref name="T"/>.</returns>
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    /// <summary>
    /// Ottiene un'entità per ID.
    /// </summary>
    /// <param name="id">ID dell'entità da cercare.</param>
    ///
