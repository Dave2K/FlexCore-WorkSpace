namespace FlexCore.Northwind;

/// <summary>
/// Interfaccia generica per il repository che gestisce le operazioni CRUD su un'entità.
/// </summary>
/// <typeparam name="T">Tipo dell'entità di destinazione.</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Aggiunge una nuova entità al database.
    /// </summary>
    /// <param name="entity">Entità da aggiungere.</param>
    /// <returns>Il risultato dell'operazione di salvataggio.</returns>
    Task AddAsync(T entity);

    /// <summary>
    /// Ottiene tutte le entità dal database.
    /// </summary>
    /// <returns>Lista di entità di tipo <typeparamref name="T"/>.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Ottiene un'entità per ID.
    /// </summary>
    /// <param name="id">ID dell'entità da cercare.</param>
    /// <returns>Entità trovata.</returns>
    Task<T?> GetByIdAsync(object id);

    /// <summary>
    /// Modifica un'entità nel database.
    /// </summary>
    /// <param name="entity">Entità da modificare.</param>
    /// <returns>Il risultato dell'operazione di salvataggio.</returns>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Elimina un'entità dal database.
    /// </summary>
    /// <param name="id">ID dell'entità da eliminare.</param>
    /// <returns>Il risultato dell'operazione di eliminazione.</returns>
    Task DeleteAsync(object id);
}
