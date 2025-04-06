using FlexCore.Database.Core.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FlexCore.Database.SQLServer
{
    /// <summary>
    /// Provider per l'interazione con SQL Server, con implementazione sincrona e asincrona.
    /// </summary>
    public class SQLServerDatabaseProvider : IDatabaseProvider
    {
        private SqlConnection? _connection;
        private SqlTransaction? _currentTransaction;

        #region Sync Methods

        public void Connect(string connectionString)
        {
            Disconnect();
            _connection = new SqlConnection(connectionString);
            _connection!.Open();
        }

        public void Disconnect()
        {
            if (_connection?.State != ConnectionState.Closed)
            {
                RollbackTransaction();
                _connection?.Close();
                _connection?.Dispose();
            }
            _connection = null;
        }

        public int ExecuteNonQuery(string command)
            => ExecuteNonQuery(command, Array.Empty<IDbDataParameter>());

        public int ExecuteNonQuery(string command, params IDbDataParameter[] parameters)
        {
            if (_connection == null)
                throw new InvalidOperationException("Connessione non inizializzata.");

            using var cmd = new SqlCommand(command, _connection, _currentTransaction);
            cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteNonQuery();
        }

        public IDataReader ExecuteQuery(string query)
            => ExecuteQuery(query, Array.Empty<IDbDataParameter>());

        public IDataReader ExecuteQuery(string query, params IDbDataParameter[] parameters)
        {
            if (_connection == null)
                throw new InvalidOperationException("Connessione non inizializzata.");

            var cmd = new SqlCommand(query, _connection, _currentTransaction);
            cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteReader();
        }

        public T ExecuteScalar<T>(string query)
        {
            if (_connection == null)
                throw new InvalidOperationException("Connessione non inizializzata.");

            using var cmd = new SqlCommand(query, _connection, _currentTransaction);
            object result = cmd.ExecuteScalar();
            return result is null || result == DBNull.Value
                ? throw new InvalidOperationException("Risultato nullo o DBNull")
                : (T)Convert.ChangeType(result, typeof(T));
        }

        public IDbDataParameter CreateParameter(string name, object value)
            => new SqlParameter(name, value);

        public IDbConnection CreateConnection(string connectionString)
            => new SqlConnection(connectionString);

        public bool IsTransactionActive()
            => _currentTransaction != null;

        public void BeginTransaction()
        {
            if (_connection == null)
                throw new InvalidOperationException("Connessione non inizializzata.");

            _currentTransaction = _connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _currentTransaction?.Commit();
            _currentTransaction = null;
        }

        public void RollbackTransaction()
        {
            _currentTransaction?.Rollback();
            _currentTransaction = null;
        }
        #endregion

        #region Async Methods

        public async Task OpenConnectionAsync(
            IDbConnection connection,
            CancellationToken cancellationToken = default)
        {
            if (connection is SqlConnection sqlConn)
                await sqlConn.OpenAsync(cancellationToken).ConfigureAwait(false);
            else
                throw new ArgumentException("Tipo connessione non supportato", nameof(connection));
        }

        public async Task<int> ExecuteNonQueryAsync(
            string command,
            CancellationToken cancellationToken = default)
        {
            if (_connection == null)
                throw new InvalidOperationException("Connessione non inizializzata.");

            using var cmd = new SqlCommand(command, _connection, _currentTransaction);
            return await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<int> ExecuteNonQueryAsync(
            string command,
            IDbDataParameter[] parameters,
            CancellationToken cancellationToken = default)
        {
            if (_connection == null)
                throw new InvalidOperationException("Connessione non inizializzata.");

            using var cmd = new SqlCommand(command, _connection, _currentTransaction);
            cmd.Parameters.AddRange(parameters);
            return await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IDataReader> ExecuteQueryAsync(
            string query,
            CancellationToken cancellationToken = default)
        {
            if (_connection == null)
                throw new InvalidOperationException("Connessione non inizializzata.");

            var cmd = new SqlCommand(query, _connection, _currentTransaction);
            return await cmd.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IDataReader> ExecuteQueryAsync(
            string query,
            IDbDataParameter[] parameters,
            CancellationToken cancellationToken = default)
        {
            if (_connection == null)
                throw new InvalidOperationException("Connessione non inizializzata.");

            var cmd = new SqlCommand(query, _connection, _currentTransaction);
            cmd.Parameters.AddRange(parameters);
            return await cmd.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<T> ExecuteScalarAsync<T>(
            string query,
            CancellationToken cancellationToken = default)
        {
            if (_connection == null)
                throw new InvalidOperationException("Connessione non inizializzata.");

            using var cmd = new SqlCommand(query, _connection, _currentTransaction);
            object? result = await cmd.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
            return result is null || result == DBNull.Value
                ? throw new InvalidOperationException("Risultato nullo o DBNull")
                : (T)Convert.ChangeType(result, typeof(T));
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_connection == null)
                throw new InvalidOperationException("Connessione non inizializzata.");

            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync(cancellationToken).ConfigureAwait(false);

            _currentTransaction = (SqlTransaction)await _connection
                .BeginTransactionAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        #endregion
    }
}