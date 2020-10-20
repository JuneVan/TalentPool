using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Une.TalentPool.Application;
using Une.TalentPool.Application.DailyStatistics;
using Dapper;

namespace Une.TalentPool.Dapper.Queriers
{
    public class DailyStatisticQuerier : IDailyStatisticQuerier
    {
        private readonly IConnectionProvider _connectionProvider;
        public DailyStatisticQuerier(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<List<DailyStatisticDto>> GetDailyStatisticsAsync(DateTime date)
        {
            using (var connection = await _connectionProvider.GetDbConnectionAsync())
            {
                var dtos = await connection.QueryAsync<DailyStatisticDto>("select Id,CreationTime,Date,Platform,Description from DailyStatistics where Date=@Date", new { Date = date });
                return dtos.ToList();
            }
        }

        public async Task<PaginationOutput<DailyStatisticDto>> GetListAsync(PaginationInput input)
        {
            var query = from a in _context.DailyStatistics
                        join b in _context.Users on a.CreatorUserId equals b.Id
                        select new DailyStatisticDto()
                        {
                            Id = a.Id,
                            CreationTime = a.CreationTime,
                            CreatorUserName = b.FullName,
                            Date = a.Date,
                            Platform = a.Platform
                        };
            var totalCount = await query.CountAsync();
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)input.PageSize);
            var dailyStatistics = await query.OrderByDescending(o => o.CreationTime)
                 .Skip((input.PageIndex - 1) * input.PageSize)
                .Take(input.PageSize)
                 .ToListAsync();
            return new PaginationOutput<DailyStatisticDto>(totalSize, dailyStatistics);
        }
    }
}
