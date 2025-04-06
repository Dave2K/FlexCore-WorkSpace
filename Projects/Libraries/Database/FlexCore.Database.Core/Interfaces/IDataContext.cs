// File: FlexCore.Database.Interfaces/IDataContext.cs
using System;
using System.Threading.Tasks;

namespace FlexCore.Database.Interfaces
{
    /// <summary>
    /// Interfaccia per l'accesso ai dati con gestione transazionale sincrona/asincrona
    /// </summary>
    public interface IDataContext : IDisposable
    {
        /// <summary>
        /// Avvia una transazione sincrona
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Avvia una transazione asincrona
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Conferma una transazione sincrona
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Conferma una transazione asincrona
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Annulla una transazione sincrona
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Annulla una transazione asincrona
        /// </summary>
        Task RollbackTransactionAsync();

        /// <summary>
        /// Salva le modifiche sincrone
        /// </summary>
        int SaveChanges();

        /// <summary>
        /// Salva le modifiche asincrone
        /// </summary>
        Task<int> SaveChangesAsync();
    }
}