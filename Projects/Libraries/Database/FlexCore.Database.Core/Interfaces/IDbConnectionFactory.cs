using System.Data;
using System.Threading.Tasks;

namespace FlexCore.Database.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
        Task<IDbConnection> CreateConnectionAsync();
    }
}