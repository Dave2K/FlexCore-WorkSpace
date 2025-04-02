using FlexCore.Database.Core.Enums;

namespace FlexCore.Database.Core.Interfaces
{
    /// <summary>
    /// Interfaccia per la creazione di provider di database.
    /// </summary>
    public interface IDatabaseFactory
    {
        IDatabaseProvider CreateProvider(DatabaseType type);
    }
}