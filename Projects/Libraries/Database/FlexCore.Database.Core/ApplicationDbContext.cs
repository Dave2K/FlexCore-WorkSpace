using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using FlexCore.Database.Interfaces; // Aggiungi questo using mancante

namespace FlexCore.Database.Core
{
    /// <summary>
    /// Contesto database principale con gestione delle transazioni
    /// </summary>
    public class ApplicationDbContext : DbContext, IDataContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Implementazione esplicita per salvataggio asincrono
        Task<int> IDataContext.SaveChangesAsync() => base.SaveChangesAsync();

        /// <inheritdoc/>
        public void BeginTransaction() => Database.BeginTransaction();

        /// <inheritdoc/>
        public async Task BeginTransactionAsync() => await Database.BeginTransactionAsync().ConfigureAwait(false);

        /// <inheritdoc/>
        public void CommitTransaction() => Database.CommitTransaction();

        /// <inheritdoc/>
        public async Task CommitTransactionAsync() => await Database.CommitTransactionAsync().ConfigureAwait(false);

        /// <inheritdoc/>
        public void RollbackTransaction() => Database.RollbackTransaction();

        /// <inheritdoc/>
        public async Task RollbackTransactionAsync() => await Database.RollbackTransactionAsync().ConfigureAwait(false);

        /// <inheritdoc/>
        public override int SaveChanges() => base.SaveChanges();

        /// <inheritdoc/>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => base.SaveChangesAsync(cancellationToken);

        /// <inheritdoc/>
        public override void Dispose()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}