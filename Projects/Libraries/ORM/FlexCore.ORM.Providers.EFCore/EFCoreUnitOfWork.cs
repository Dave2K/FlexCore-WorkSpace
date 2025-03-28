namespace FlexCore.ORM.Providers.EFCore;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FlexCore.ORM.Core.Implementations;

/// <summary>
/// Implementazione di UnitOfWork per EF Core. Gestisce la transazione e il salvataggio delle modifiche utilizzando Entity Framework Core.
/// </summary>
public class EFCoreUnitOfWork : UnitOfWorkBase
{
    // Riferimento al contesto del database (DbContext)
    private readonly DbContext _dbContext;

    /// <summary>
    /// Inizializza una nuova istanza di <see cref="EFCoreUnitOfWork"/>.
    /// </summary>
    /// <param name="dbContext">Il contesto del database EF Core.</param>
    public EFCoreUnitOfWork(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Avvia una nuova transazione asincrona nel contesto di EF Core.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public override async Task BeginTransactionAsync()
    {
        await _dbContext.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// Commette la transazione asincrona nel contesto di EF Core.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public override async Task CommitTransactionAsync()
    {
        await _dbContext.Database.CommitTransactionAsync();
    }

    /// <summary>
    /// Annulla la transazione asincrona nel contesto di EF Core.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public override async Task RollbackTransactionAsync()
    {
        await _dbContext.Database.RollbackTransactionAsync();
    }

    /// <summary>
    /// Salva le modifiche nel contesto di EF Core in modo asincrono.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona, restituendo il numero di entità salvate.</returns>
    public override async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Rilascia le risorse utilizzate dall'istanza di <see cref="EFCoreUnitOfWork"/>.
    /// </summary>
    /// <param name="disposing">Indica se il metodo è stato chiamato tramite il metodo Dispose o dal finalizzatore.</param>
    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Rilascia le risorse gestite (DbContext)
                _dbContext?.Dispose();
            }

            // Rilascia le risorse non gestite (nessuna nel caso di DbContext)
            _disposed = true;
        }
    }
}
