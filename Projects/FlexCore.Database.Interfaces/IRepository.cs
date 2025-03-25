namespace FlexCore.Database.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Interfaccia generica per il repository pattern.
/// </summary>
/// <typeparam name="TEntity">Tipo dell'entità gestita dal repository.</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Recupera tutte le entità.
    /// </summary>
    /// <returns>Una collezione di entità.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Recupera un'entità in base all'identificatore.
    /// </summary>
    /// <param name="id">Identificatore univoco dell'entità.</param>
    /// <returns>L'entità corrispondente se trovata, altrimenti null.</returns>
    Task<TEntity?> GetByIdAsync(Guid id);

    /// <summary>
    /// Aggiunge una nuova entità al repository.
    /// </summary>
    /// <param name="entity">L'entità da aggiungere.</param>
    Task AddAsync(TEntity entity);

    /// <summary>
    /// Aggiorna un'entità esistente.
    /// </summary>
    /// <param name="entity">L'entità con i dati aggiornati.</param>
    void Update(TEntity entity);

    /// <summary>
    /// Rimuove un'entità dal repository.
    /// </summary>
    /// <param name="entity">L'entità da eliminare.</param>
    void Delete(TEntity entity);
}