using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Une.TalentPool.Dapper
{
    public interface IConnectionProvider : IDisposable
    {
        Task<DbConnection> GetDbConnectionAsync();
    }
}
