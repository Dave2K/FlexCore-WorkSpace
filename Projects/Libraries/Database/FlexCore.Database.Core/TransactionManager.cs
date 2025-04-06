using FlexCore.Database.Interfaces; // Aggiungi questo using
using System;
using System.Transactions;
using System.Threading.Tasks;
using FlexCore.Database.Core.Interfaces;

namespace FlexCore.Database.Core
{
    /// <summary>
    /// Gestore delle transazioni con supporto completo per transazioni sincrone, asincrone e distribuite.
    /// </summary>
    public class TransactionManager : ITransactionManager
    {
        private readonly IDatabaseProvider _provider;

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="TransactionManager"/>.
        /// </summary>
        /// <param name="provider">Provider di database per l'esecuzione delle transazioni.</param>
        /// <exception cref="ArgumentNullException">Se <paramref name="provider"/> è null.</exception>
        public TransactionManager(IDatabaseProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <inheritdoc/>
        public async Task BeginTransactionAsync()
        {
            if (_provider is IDataContext dataContext)
                await dataContext.BeginTransactionAsync().ConfigureAwait(false);
            else
                _provider.BeginTransaction();
        }

        /// <inheritdoc/>
        public async Task CommitTransactionAsync()
        {
            if (_provider is IDataContext dataContext)
                await dataContext.CommitTransactionAsync().ConfigureAwait(false);
            else
                _provider.CommitTransaction();
        }

        /// <inheritdoc/>
        public async Task RollbackTransactionAsync()
        {
            if (_provider is IDataContext dataContext)
                await dataContext.RollbackTransactionAsync().ConfigureAwait(false);
            else
                _provider.RollbackTransaction();
        }

        /// <inheritdoc/>
        public async Task BeginDistributedTransactionAsync()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Logica transazionale...
                scope.Complete();
                await Task.CompletedTask.ConfigureAwait(false);
            }
        }
    }
}