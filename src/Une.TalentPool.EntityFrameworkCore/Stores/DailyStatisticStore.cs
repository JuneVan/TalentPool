using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Une.TalentPool.DailyStatistics;

namespace Une.TalentPool.EntityFrameworkCore.Stores
{
    public class DailyStatisticStore : StoreBase, IDailyStatisticStore
    {
        public DailyStatisticStore(VanDbContext context)
              : base(context)
        {
        }
 
        public async Task<DailyStatistic> CreateAsync(DailyStatistic dailyStatistic, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (dailyStatistic == null)
                throw new ArgumentNullException(nameof(dailyStatistic));

            Context.DailyStatistics.Add(dailyStatistic);
            await SaveChanges(cancellationToken);
            return dailyStatistic;
        }

        public async Task<DailyStatistic> DeleteAsync(DailyStatistic dailyStatistic, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (dailyStatistic == null)
                throw new ArgumentNullException(nameof(dailyStatistic));
            // 清除子实体集合防止更新异常
            var dailyStatisticDetails = await Context.DailyStatisticItems.Where(w => w.DailyStatisticId == dailyStatistic.Id).ToListAsync();
            if (dailyStatisticDetails != null)
            {
                foreach (var dailyStatisticDetail in dailyStatisticDetails)
                {
                    Context.DailyStatisticItems.Remove(dailyStatisticDetail);
                }
            }

            Context.DailyStatistics.Update(dailyStatistic);

            await SaveChanges(cancellationToken);
            return dailyStatistic;
        }

        public async Task<DailyStatistic> FindByIdAsync(Guid dailyStatisticId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (dailyStatisticId == null)
                throw new ArgumentNullException(nameof(dailyStatisticId));
            return await Context.DailyStatistics.Include(i => i.Items).FirstOrDefaultAsync(f => f.Id == dailyStatisticId);
        }

        public async Task<DailyStatistic> UpdateAsync(DailyStatistic dailyStatistic, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (dailyStatistic == null)
                throw new ArgumentNullException(nameof(dailyStatistic));

            // 清除子实体集合防止更新异常
            var dailyStatisticDetails = await Context.DailyStatisticItems.Where(w => w.DailyStatisticId == dailyStatistic.Id).ToListAsync();
            if (dailyStatisticDetails != null)
            {
                foreach (var dailyStatisticDetail in dailyStatisticDetails)
                {
                    Context.DailyStatisticItems.Remove(dailyStatisticDetail);
                }
            }


            Context.DailyStatistics.Update(dailyStatistic);
            await SaveChanges(cancellationToken);
            return dailyStatistic;
        }
    }
}
