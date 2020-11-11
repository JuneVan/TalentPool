using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TalentPool.Application.DailyStatistics
{
    public interface IDailyStatisticQuerier
    {
        Task<PaginationOutput<DailyStatisticDto>> GetListAsync(PaginationInput input);
        Task<List<DailyStatisticDto>> GetDailyStatisticsAsync(DateTime date);
        Task<List<DailyStatisticChartDto>> GetChartStatisticsAsync(DateTime startTime, DateTime endTime);
    }
}
