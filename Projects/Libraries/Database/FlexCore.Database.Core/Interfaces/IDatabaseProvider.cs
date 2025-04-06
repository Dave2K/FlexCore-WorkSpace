using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FlexCore.Database.Core.Interfaces
{
    /// <summary>
    /// Interfaccia base per tutti i provider di database, con supporto sincrono e asincrono.
    /// </summary>
    public interface IDatabaseProvider
    {
        // Metodi Sincroni

        /// <summary>
        /// Apre una connessione al database in modo sincrono.
        /// </summary>
        /// <param name="connectionString">Stringa di connessione.</param>
        void Connect(string connectionString);

        /// <summary>
        /// Chiude la connessione corrente in modo sincrono.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Esegue un comando SQL senza parametri in modo sincrono.
        /// </summary>
        /// <returns>Numero di righe interessate.</returns>
        int ExecuteNonQuery(string command);

        /// <summary>
        /// Esegue un comando SQL con parametri in modo sincrono.
        /// </summary>
        int ExecuteNonQuery(string command, params IDbDataParameter[] parameters);

        /// <summary>
        /// Esegue una query SQL senza parametri in modo sincrono.
        /// </summary>
        IDataReader ExecuteQuery(string query);

        /// <summary>
        /// Esegue una query SQL con parametri in modo sincrono.
        /// </summary>
        IDataReader ExecuteQuery(string query, params IDbDataParameter[] parameters);

        /// <summary>
        /// Esegue una query e restituisce un singolo valore in modo sincrono.
        /// </summary>
        T ExecuteScalar<T>(string query);

        /// <summary>
        /// Crea un parametro per query in modo sincrono.
        /// </summary>
        IDbDataParameter CreateParameter(string name, object value);

        /// <summary>
        /// Crea una connessione non aperta in modo sincrono.
        /// </summary>
        IDbConnection CreateConnection(string connectionString);

        /// <summary>
        /// Verifica se è presente una transazione attiva.
        /// </summary>
        bool IsTransactionActive();

        /// <summary>
        /// Avvia una nuova transazione in modo sincrono.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Conferma la transazione corrente in modo sincrono.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Annulla la transazione corrente in modo sincrono.
        /// </summary>
        void RollbackTransaction();

        // Metodi Asincroni

        /// <summary>
        /// Apre una connessione al database in modo asincrono.
        /// </summary>
        /// <param name="connection">Connessione da aprire.</param>
        /// <param name="cancellationToken">Token per annullamento operazione.</param>
        Task OpenConnectionAsync(IDbConnection connection, CancellationToken cancellationToken = default);

        /// <summary>
        /// Esegue un comando SQL senza parametri in modo asincrono.
        /// </summary>
        /// <param name="cancellationToken">Token per annullamento operazione.</param>
        Task<int> ExecuteNonQueryAsync(string command, CancellationToken cancellationToken = default);

        /// <summary>
        /// Esegue un comando SQL con parametri in modo asincrono.
        /// </summary>
        Task<int> ExecuteNonQueryAsync(string command, IDbDataParameter[] parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Esegue una query SQL senza parametri in modo asincrono.
        /// </summary>
        Task<IDataReader> ExecuteQueryAsync(string query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Esegue una query SQL con parametri in modo asincrono.
        /// </summary>
        Task<IDataReader> ExecuteQueryAsync(string query, IDbDataParameter[] parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Esegue una query e restituisce un singolo valore in modo asincrono.
        /// </summary>
        Task<T> ExecuteScalarAsync<T>(string query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Avvia una nuova transazione in modo asincrono.
        /// </summary>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}