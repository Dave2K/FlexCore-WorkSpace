using System.Threading.Tasks;

namespace FlexCore.Database.Core.Interfaces
{
    /// <summary>
    /// Interfaccia per operazioni asincrone aggiuntive
    /// </summary>
    public interface IAsyncDbProvider
    {
        /// <summary>
        /// Apre una connessione in modo asincrono
        /// </summary>
        Task OpenConnectionAsync();

        /// <summary>
        /// Avvia una transazione asincrona
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Conferma una transazione asincrona
        /// </summary>
        Task CommitTransactionAsync();
    }
}