using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace TalentPool.Dapper
{
    public interface IConnectionProvider : IDisposable
    {
        Task<DbConnection> GetDbConnectionAsync();
    }
}
