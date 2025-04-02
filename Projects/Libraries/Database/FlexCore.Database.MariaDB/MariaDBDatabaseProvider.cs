using FlexCore.Database.Core.Interfaces;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;

namespace FlexCore.Database.MariaDB
{
    /// <summary>
    /// Implementazione concreta di <see cref="IDatabaseProvider"/> per MariaDB.
    /// </summary>
    public class MariaDBDatabaseProvider : IDatabaseProvider
    {
        private MySqlConnection? _connection;

        /// <summary>
        /// Apre una connessione al database MariaDB.
        /// </summary>
        /// <param name="connectionString">Stringa di connessione.</param>
        public void Connect(string connectionString)
        {
            Disconnect();
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
        }

        /// <summary>
        /// Esegue una query e restituisce un IDataReader.
        /// </summary>
        /// <param name="query">Query SQL da eseguire.</param>
        public IDataReader ExecuteQuery(string query)
        {
            using var command = new MySqlCommand(query, _connection);
            return command.ExecuteReader();
        }

        /// <summary>
        /// Esegue un comando non query.
        /// </summary>
        /// <param name="command">Comando SQL da eseguire.</param>
        public int ExecuteNonQuery(string command)
        {
            using var cmd = new MySqlCommand(command, _connection);
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
            => new MySqlParameter(name, value);

        /// <summary>
        /// Crea una connessione non aperta.
        /// </summary>
        /// <param name="connectionString">Stringa di connessione.</param>
        public IDbConnection CreateConnection(string connectionString)
            => new MySqlConnection(connectionString);

        /// <summary>
        /// Apre una connessione in modo asincrono.
        /// </summary>
        /// <param name="connection">Connessione da aprire.</param>
        public async Task OpenConnectionAsync(IDbConnection connection)
        {
            if (connection is MySqlConnection mysqlConn)
                await mysqlConn.OpenAsync();
            else
                throw new ArgumentException("Connessione non valida per MariaDB");
        }
    }
}