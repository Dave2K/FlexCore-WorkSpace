using FlexCore.Database.Core.Interfaces;
using System;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace FlexCore.Database.SQLite
{
    /// <summary>
    /// Implementazione concreta di <see cref="IDatabaseProvider"/> per SQLite.
    /// </summary>
    public class SQLiteDatabaseProvider : IDatabaseProvider
    {
        private SQLiteConnection? _connection;

        /// <summary>
        /// Apre una connessione al database SQLite.
        /// </summary>
        /// <param name="connectionString">Stringa di connessione.</param>
        public void Connect(string connectionString)
        {
            Disconnect();
            _connection = new SQLiteConnection(connectionString);
            _connection.Open();
        }

        /// <summary>
        /// Esegue una query e restituisce un IDataReader.
        /// </summary>
        /// <param name="query">Query SQL da eseguire.</param>
        public IDataReader ExecuteQuery(string query)
        {
            using var command = new SQLiteCommand(query, _connection);
            return command.ExecuteReader();
        }

        /// <summary>
        /// Esegue un comando non query.
        /// </summary>
        /// <param name="command">Comando SQL da eseguire.</param>
        public int ExecuteNonQuery(string command)
        {
            using var cmd = new SQLiteCommand(command, _connection);
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Chiude la connessione al database.
        /// </summary>
        public void Disconnect()
        {
            if (_connection?.State != ConnectionState.Closed)
            {
                _connection?.Close();
                _connection?.Dispose();
            }
            _connection = null;
        }

        /// <summary>
        /// Crea un parametro per query.
        /// </summary>
        /// <param name="name">Nome del parametro.</param>
        /// <param name="value">Valore del parametro.</param>
        public IDbDataParameter CreateParameter(string name, object value)
            => new SQLiteParameter(name, value);

        /// <summary>
        /// Crea una connessione non aperta.
        /// </summary>
        /// <param name="connectionString">Stringa di connessione.</param>
        public IDbConnection CreateConnection(string connectionString)
            => new SQLiteConnection(connectionString);

        /// <summary>
        /// Apre una connessione in modo asincrono.
        /// </summary>
        /// <param name="connection">Connessione da aprire.</param>
        public async Task OpenConnectionAsync(IDbConnection connection)
        {
            if (connection is SQLiteConnection sqliteConn)
                await sqliteConn.OpenAsync();
            else
                throw new ArgumentException("Connessione non valida per SQLite");
        }
    }
}