using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Une.TalentPool.Application;
using Une.TalentPool.Application.DailyStatistics;

namespace Une.TalentPool.EntityFrameworkCore.Queriers
{
    public class DailyStatisticQuerier : IDailyStatisticQuerier
    {
        private readonly TalentDbContext _context;
        private readonly ICancellationTokenProvider _tokenProvider;
        public DailyStatisticQuerier(TalentDbContext context,
            ICancellationTokenProvider tokenProvider)
        {
            _context = context;
            _tokenProvider = tokenProvider;
        }
        protected CancellationToken CancellationToken => _tokenProvider.Token;

        public async Task<List<DailyStatisticChartDto>> GetChartStatisticsAsync(DateTime startTime, DateTime endTime)
        {
            CancellationToken.ThrowIfCancellationRequested();
            if (startTime == null)
                throw new ArgumentNullException(nameof(startTime));
            if (endTime == null)
                throw new ArgumentNullException(nameof(endTime));

            var query = from a in _context.DailyStatistics
                        join b in _context.DailyStatisticItems on a.Id equals b.DailyStatisticId
                        select new DailyStatisticChartDto()
                        {
                            Date = a.Date,
                            Platform = a.Platform,
                            JobName = b.JobName,
                            UpdateCount = b.UpdateCount,
                            DownloadCount = b.DownloadCount
                        };
            return await query.ToListAsync(CancellationToken);
        }

        public async Task<List<DailyStatisticDto>> GetDailyStatisticsAsync(DateTime date)
        {
            CancellationToken.ThrowIfCancellationRequested();
            if (date == null)
                throw new ArgumentNullException(nameof(date)); 

            return await _context.DailyStatistics
                .Where(w => w.Date == date)
                .Select(s => new DailyStatisticDto()
                {
                    Id = s.Id,
                    CreationTime = s.CreationTime,
                    Date = s.Date,
                    Platform = s.Platform,
                    Description = s.Description
                })
                .ToListAsync(CancellationToken);
        }

        public async Task<PaginationOutput<DailyStatisticDto>> GetListAsync(PaginationInput input)
        {
            CancellationToken.ThrowIfCancellationRequested();
            if (input == null)
                throw new ArgumentNullException(nameof(input));

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
            var totalCount = await query.CountAsync(CancellationToken);
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)input.PageSize);
            var dailyStatistics = await query.OrderByDescending(o => o.CreationTime)
                 .Skip((input.PageIndex - 1) * input.PageSize)
                .Take(input.PageSize)
                 .ToListAsync(CancellationToken);
            return new PaginationOutput<DailyStatisticDto>(totalSize, dailyStatistics);
        }
    }
}
