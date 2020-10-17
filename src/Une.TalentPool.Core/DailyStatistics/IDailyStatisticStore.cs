using System;
using System.Threading;
using System.Threading.Tasks;

namespace Une.TalentPool.DailyStatistics
{
    public interface IDailyStatisticStore : IDisposable
    {
        Task<DailyStatistic> CreateAsync(DailyStatistic dailyStatistic, CancellationToken cancellationToken);
        Task<DailyStatistic> DeleteAsync(DailyStatistic dailyStatistic, CancellationToken cancellationToken);
        Task<DailyStatistic> UpdateAsync(DailyStatistic dailyStatistic, CancellationToken cancellationToken);
        Task<DailyStatistic> FindByIdAsync(Guid dailyStatisticId, CancellationToken cancellationToken);
    }
}
