namespace FlexCore.Database.SQLite;

using FlexCore.Database.Core;
using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Threading.Tasks;

/// <summary>
/// Implementazione del provider per SQLite.
/// </summary>
public class SQLiteProvider : DbConnectionFactory
{
    private new readonly string _connectionString;

    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="SQLiteProvider"/>.
    /// </summary>
    /// <param name="connectionString">Stringa di connessione al database.</param>
    public SQLiteProvider(string connectionString) : base(connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString), "La stringa di connessione non può essere vuota.");

        _connectionString = connectionString;
    }

    /// <summary>
    /// Crea una connessione al database SQLite.
    /// </summary>
    /// <returns>Connessione al database.</returns>
    public override IDbConnection CreateConnection()
    {
        if (string.IsNullOrEmpty(_connectionString))
            throw new InvalidOperationException("La stringa di connessione non è stata configurata.");

        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    /// <summary>
    /// Crea una connessione al database SQLite in modo asincrono.
    /// </summary>
    /// <returns>Connessione al database.</returns>
    public override async Task<IDbConnection> CreateConnectionAsync()
    {
        if (string.IsNullOrEmpty(_connectionString))
            throw new InvalidOperationException("La stringa di connessione non è stata configurata.");

        var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}