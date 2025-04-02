using FlexCore.Database.Core.Enums;
using FlexCore.Database.Core.Interfaces;
using FlexCore.Database.SQLite;
using FlexCore.Database.SQLServer;
using FlexCore.Database.MariaDB;
using System;
using FlexCore.Database.Core;

namespace FlexCore.Database.Factory
{
    /// <summary>
    /// Factory per creare istanze concrete di IDatabaseProvider.
    /// </summary>
    public static class DatabaseFactory // Classe statica per risolvere CA1822
    {
        /// <summary>
        /// Crea un provider di database specifico in base al tipo richiesto.
        /// </summary>
        public static IDatabaseProvider CreateProvider(DatabaseType type)
        {
            return type switch
            {
                DatabaseType.SQLite => new SQLiteDatabaseProvider(),
                DatabaseType.SQLServer => new SQLServerDatabaseProvider(),
                DatabaseType.MariaDB => new MariaDBDatabaseProvider(),
                _ => throw new NotSupportedException($"Tipo database non supportato: {type}")
            };
        }
    }
}