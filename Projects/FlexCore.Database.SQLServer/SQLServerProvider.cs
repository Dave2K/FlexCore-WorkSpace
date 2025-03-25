namespace FlexCore.Database.SQLServer;

using FlexCore.Database.Core;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

/// <summary>
/// Implementazione del provider per Microsoft SQL Server.
/// </summary>
public class SQLServerProvider : DbConnectionFactory
{
    /// <summary>
    /// Inizializza una nuova istanza del provider per SQL Server.
    /// </summary>
    /// <param name="connectionString">Stringa di connessione al database.</param>
    public SQLServerProvider(string connectionString) : base(connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString), "La stringa di connessione non può essere vuota.");
    }

    /// <summary>
    /// Crea una connessione aperta a SQL Server.
    /// </summary>
    /// <returns>Connessione aperta al database.</returns>
    public override IDbConnection CreateConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }

    /// <summary>
    /// Crea una connessione aperta a SQL Server in modo asincrono.
    /// </summary>
    /// <returns>Task che restituisce la connessione aperta.</returns>
    public override async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}