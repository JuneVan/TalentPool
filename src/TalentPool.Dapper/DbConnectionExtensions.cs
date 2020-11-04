using Dapper;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TalentPool.Application;

namespace TalentPool.Dapper
{
    public static class DbConnectionExtensions
    {
        public static async Task<PaginationOutput<TDto>> GetPaginationAsync<TDto>(this DbConnection connection, string columnSql, string fromSql, object parameter, int pageSize, int pageIndex)
        {
            var totalCount = await connection.ExecuteScalarAsync<int>($"SELECT COUNT(1) {fromSql};", parameter);
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)pageSize);
            var dtos = await connection.QueryAsync<TDto>($"SELECT {columnSql} {fromSql} LIMIT {(pageIndex - 1) * pageSize},{pageSize};", parameter);
            return new PaginationOutput<TDto>(totalSize, dtos.ToList());
        }
    }
}
