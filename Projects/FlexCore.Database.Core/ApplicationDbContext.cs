namespace FlexCore.Database.Core;

using FlexCore.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

/// <summary>
/// Contesto database principale con gestione delle transazioni.
/// </summary>
public class ApplicationDbContext : DbContext, IUnitOfWork
{
    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="ApplicationDbContext"/>.
    /// </summary>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Avvia una transazione nel database.
    /// </summary>
    public void BeginTransaction() => Database.BeginTransaction();

    /// <summary>
    /// Conferma la transazione in corso.
    /// </summary>
    public void CommitTransaction()
    {
        if (Database.CurrentTransaction != null)
        {
            Database.CurrentTransaction.Commit();
            Database.CurrentTransaction.Dispose();
        }
    }

    /// <summary>
    /// Annulla la transazione in corso.
    /// </summary>
    public void RollbackTransaction()
    {
        if (Database.CurrentTransaction != null)
        {
            Database.CurrentTransaction.Rollback();
            Database.CurrentTransaction.Dispose();
        }
    }

    /// <summary>
    /// Salva le modifiche nel database in modo asincrono.
    /// </summary>
    public async Task<int> SaveChangesAsync()
        => await base.SaveChangesAsync().ConfigureAwait(false);

    /// <summary>
    /// Rilascia le risorse gestite.
    /// </summary>
    public override void Dispose()
    {
        base.Dispose(); // Eliminazione gestita da EF Core
        GC.SuppressFinalize(this);
    }
}