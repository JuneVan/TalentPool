using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Van.TalentPool.Application.DailyStatistics
{
    public interface IDailyStatisticQuerier
    {
        Task<PaginationOutput<DailyStatisticDto>> GetListAsync(PaginationInput input);
        Task<List<DailyStatisticDto>> GetDailyStatisticsAsync(DateTime date);
    }
}
