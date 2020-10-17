using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Une.TalentPool.Application;
using Une.TalentPool.Application.DailyStatistics;

namespace Une.TalentPool.EntityFrameworkCore.Queriers
{
    public class DailyStatisticQuerier : IDailyStatisticQuerier
    {
        private readonly VanDbContext _context;
        public DailyStatisticQuerier(VanDbContext context)
        {
            _context = context;
        }

        public async Task<List<DailyStatisticDto>> GetDailyStatisticsAsync(DateTime date)
        {
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
                .ToListAsync();
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
