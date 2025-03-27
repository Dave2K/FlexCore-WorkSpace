namespace FlexCore.Database.Core;

using FlexCore.Database.Interfaces;
using System;

/// <summary>
/// Implementazione base del pattern Unit of Work con gestione semplificata delle risorse.
/// </summary>
public abstract class UnitOfWorkBase : IUnitOfWork
{
    private readonly IDataContext _dataContext;
    private bool _disposed;

    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="UnitOfWorkBase"/>.
    /// </summary>
    /// <param name="dataContext">Contesto di dati associato all'unità di lavoro.</param>
    protected UnitOfWorkBase(IDataContext dataContext)
    {
        _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
    }

    // Delega tutte le operazioni di transazione al contesto sottostante
    public void BeginTransaction() => _dataContext.BeginTransaction();
    public void CommitTransaction() => _dataContext.CommitTransaction();
    public void RollbackTransaction() => _dataContext.RollbackTransaction();

    // Metodi per il salvataggio delle modifiche
    public int SaveChanges() => _dataContext.SaveChanges();
    public async Task<int> SaveChangesAsync() => await _dataContext.SaveChangesAsync();

    /// <summary>
    /// Rilascia le risorse gestite.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Rilascia le risorse gestite e, opzionalmente, quelle non gestite.
    /// </summary>
    /// <param name="disposing">
    /// Se true, rilascia sia le risorse gestite che quelle non gestite; 
    /// se false, rilascia solo le risorse non gestite.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Elimina esplicitamente il contesto dati (senza cast ridondanti)
                _dataContext.Dispose();
            }
            _disposed = true;
        }
    }
}