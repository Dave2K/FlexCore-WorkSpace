using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using FlexCore.ORM.Core.Interfaces;

namespace FlexCore.ORM.Providers.ADO
{
    public class AdoOrmProvider(IDbConnection connection) : IOrmProvider, IDisposable
    {
        private readonly IDbConnection _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        private IDbTransaction? _transaction = null;

        public async Task<T?> GetByIdAsync<T>(Guid id) where T : class
        {
            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {typeof(T).Name} WHERE Id = @Id";

            var param = command.CreateParameter();
            param.ParameterName = "@Id";
            param.Value = id;
            command.Parameters.Add(param);

            using var reader = await ExecuteReaderAsync(command);
            return await ReadEntityAsync<T>(reader);
        }

        public async Task AddAsync<T>(T entity) where T : class
        {
            var query = GenerateInsertQuery<T>();
            using var command = _connection.CreateCommand();
            command.CommandText = query;

            foreach (var prop in typeof(T).GetProperties())
            {
                var param = command.CreateParameter();
                param.ParameterName = $"@{prop.Name}";
                param.Value = prop.GetValue(entity) ?? DBNull.Value;
                command.Parameters.Add(param);
            }

            await ExecuteNonQueryAsync(command);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {typeof(T).Name}";

            using var reader = await ExecuteReaderAsync(command);
            var results = new List<T>();

            while (await reader.ReadAsync())
            {
                results.Add(MapToEntity<T>(reader));
            }
            return results;
        }

        public async Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                await AddAsync(entity);
            }
        }

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            var setClause = string.Join(", ",
                typeof(T).GetProperties()
                .Where(p => p.Name != "Id")
                .Select(p => $"{p.Name} = @{p.Name}"));

            using var command = _connection.CreateCommand();
            command.CommandText = $"UPDATE {typeof(T).Name} SET {setClause} WHERE Id = @Id";

            foreach (var prop in typeof(T).GetProperties())
            {
                var param = command.CreateParameter();
                param.ParameterName = $"@{prop.Name}";
                param.Value = prop.GetValue(entity) ?? DBNull.Value;
                command.Parameters.Add(param);
            }

            await ExecuteNonQueryAsync(command);
        }

        public async Task UpdateRangeAsync<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity);
            }
        }

        public async Task DeleteAsync<T>(T entity) where T : class
        {
            var id = typeof(T).GetProperty("Id")?.GetValue(entity);
            using var command = _connection.CreateCommand();
            command.CommandText = $"DELETE FROM {typeof(T).Name} WHERE Id = @Id";

            var param = command.CreateParameter();
            param.ParameterName = "@Id";
            param.Value = id;
            command.Parameters.Add(param);

            await ExecuteNonQueryAsync(command);
        }

        public async Task DeleteRangeAsync<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                await DeleteAsync(entity);
            }
        }

        public Task<int> SaveChangesAsync() => Task.FromResult(0);

        public async Task BeginTransactionAsync()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            _transaction = _connection.BeginTransaction();
            await Task.CompletedTask;
        }

        public async Task CommitTransactionAsync()
        {
            _transaction?.Commit();
            _transaction = null;
            await Task.CompletedTask;
        }

        public async Task RollbackTransactionAsync()
        {
            _transaction?.Rollback();
            _transaction = null;
            await Task.CompletedTask;
        }

        private async Task<DbDataReader> ExecuteReaderAsync(IDbCommand command)
        {
            // CA1859: Utilizzo di tipo concreto DbCommand
            if (command is DbCommand dbCommand)
            {
                return await dbCommand.ExecuteReaderAsync();
            }

            // CS0266: Cast esplicito per IDataReader
            return (DbDataReader)await Task.Run(() => command.ExecuteReader());
        }

        private async Task<int> ExecuteNonQueryAsync(IDbCommand command)
        {
            // CA1859: Utilizzo di tipo concreto DbCommand
            if (command is DbCommand dbCommand)
            {
                return await dbCommand.ExecuteNonQueryAsync();
            }

            return await Task.Run(() => command.ExecuteNonQuery());
        }

        // CA1822: Metodo reso static
        private static async Task<T?> ReadEntityAsync<T>(DbDataReader reader) where T : class
        {
            return await reader.ReadAsync() ? MapToEntity<T>(reader) : null;
        }

        // CA1822: Metodo reso static
        private static T MapToEntity<T>(IDataRecord reader) where T : class
        {
            var entity = Activator.CreateInstance<T>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var prop = typeof(T).GetProperty(reader.GetName(i));
                prop?.SetValue(entity, reader.IsDBNull(i) ? null : reader.GetValue(i));
            }
            return entity;
        }

        // CA1822: Metodo reso static
        private static string GenerateInsertQuery<T>()
        {
            var properties = typeof(T).GetProperties();
            var columns = string.Join(", ", properties.Select(p => p.Name));
            var parameters = string.Join(", ", properties.Select(p => $"@{p.Name}"));
            return $"INSERT INTO {typeof(T).Name} ({columns}) VALUES ({parameters})";
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}