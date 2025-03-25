namespace FlexCore.ORM.Providers.EFCore;

using FlexCore.ORM.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Implementazione del provider ORM basato su Entity Framework Core.
/// Questa classe fornisce metodi per interagire con il database tramite EF Core, come aggiungere, aggiornare, eliminare e recuperare entit�.
/// </summary>
public class EFCoreOrmProvider : IOrmProvider
{
    // Riferimento al contesto del database (DbContext)
    private readonly DbContext _dbContext;

    /// <summary>
    /// Inizializza una nuova istanza di <see cref="EFCoreOrmProvider"/>.
    /// </summary>
    /// <param name="dbContext">Il contesto del database EF Core.</param>
    /// <exception cref="ArgumentNullException">Lanciato se il <paramref name="dbContext"/> � null.</exception>
    public EFCoreOrmProvider(DbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>
    /// Recupera un'entit� per il suo ID in modo asincrono.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entit� da recuperare.</typeparam>
    /// <param name="id">L'ID dell'entit� da recuperare.</param>
    /// <returns>Un task che rappresenta l'operazione asincrona. Restituisce l'entit� se trovata, altrimenti null.</returns>
    public async Task<T?> GetByIdAsync<T>(Guid id) where T : class
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    /// <summary>
    /// Recupera tutte le entit� di un tipo in modo asincrono.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entit� da recuperare.</typeparam>
    /// <returns>Un task che rappresenta l'operazione asincrona. Restituisce una collezione di entit�.</returns>
    public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
    {
        return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Aggiunge una nuova entit� al contesto di dati in modo asincrono.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entit� da aggiungere.</typeparam>
    /// <param name="entity">L'entit� da aggiungere.</param>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public async Task AddAsync<T>(T entity) where T : class
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Aggiunge un intervallo di entit� al contesto di dati in modo asincrono.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entit� da aggiungere.</typeparam>
    /// <param name="entities">Le entit� da aggiungere.</param>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public async Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class
    {
        await _dbContext.Set<T>().AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Aggiorna un'entit� nel contesto di dati in modo asincrono.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entit� da aggiornare.</typeparam>
    /// <param name="entity">L'entit� da aggiornare.</param>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public async Task UpdateAsync<T>(T entity) where T : class
    {
        _dbContext.Set<T>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Aggiorna un intervallo di entit� nel contesto di dati in modo asincrono.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entit� da aggiornare.</typeparam>
    /// <param name="entities">Le entit� da aggiornare.</param>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public async Task UpdateRangeAsync<T>(IEnumerable<T> entities) where T : class
    {
        _dbContext.Set<T>().UpdateRange(entities);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Elimina un'entit� dal contesto di dati in modo asincrono.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entit� da eliminare.</typeparam>
    /// <param name="entity">L'entit� da eliminare.</param>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public async Task DeleteAsync<T>(T entity) where T : class
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Elimina un intervallo di entit� dal contesto di dati in modo asincrono.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entit� da eliminare.</typeparam>
    /// <param name="entities">Le entit� da eliminare.</param>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public async Task DeleteRangeAsync<T>(IEnumerable<T> entities) where T : class
    {
        _dbContext.Set<T>().RemoveRange(entities);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Salva tutte le modifiche nel contesto di dati in modo asincrono.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona. Restituisce il numero di entit� salvate.</returns>
    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Avvia una transazione asincrona nel contesto di EF Core.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public async Task BeginTransactionAsync()
    {
        await _dbContext.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// Commette la transazione asincrona nel contesto di EF Core.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public async Task CommitTransactionAsync()
    {
        await _dbContext.Database.CommitTransactionAsync();
    }

    /// <summary>
    /// Annulla la transazione asincrona nel contesto di EF Core.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public async Task RollbackTransactionAsync()
    {
        await _dbContext.Database.RollbackTransactionAsync();
    }

    /// <summary>
    /// Rilascia le risorse utilizzate dal provider ORM.
    /// </summary>
    public void Dispose()
    {
        _dbContext?.Dispose();
    }
}
