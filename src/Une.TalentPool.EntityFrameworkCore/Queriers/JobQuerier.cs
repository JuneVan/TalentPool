using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Une.TalentPool.Application;
using Une.TalentPool.Application.Jobs;

namespace Une.TalentPool.EntityFrameworkCore.Queriers
{
    public class JobQuerier : IJobQuerier
    {
        private readonly TalentDbContext _context;
        private readonly ICancellationTokenProvider _tokenProvider;
        public JobQuerier(TalentDbContext context,
            ICancellationTokenProvider tokenProvider)
        {
            _context = context;
            _tokenProvider = tokenProvider;
        }
        protected CancellationToken CancellationToken => _tokenProvider.Token;

        public async Task<PaginationOutput<JobDto>> GetListAsync(QueryJobInput input)
        {
            CancellationToken.ThrowIfCancellationRequested();
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var query = from a in _context.Jobs
                        select new JobDto()
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Enable = a.Enable,
                            AgeRange = a.AgeRange,
                            SalaryRange = a.SalaryRange,
                            GenderRange = a.GenderRange,
                            CreationTime = a.CreationTime
                        };

            var totalCount = await query.CountAsync(CancellationToken);
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)input.PageSize);
            var jobs = await query.OrderByDescending(o => o.CreationTime)
                 .Skip((input.PageIndex - 1) * input.PageSize)
                .Take(input.PageSize).ToListAsync(CancellationToken);

            return new PaginationOutput<JobDto>(totalSize, jobs);
        }
        public async Task<List<JobSelectItemDto>> GetJobsAsync()
        {
            CancellationToken.ThrowIfCancellationRequested(); 

            var query = from a in _context.Jobs
                        where a.Enable == true
                        select new JobSelectItemDto()
                        {
                            Id = a.Id,
                            Title = a.Title
                        };
            return await query.ToListAsync(CancellationToken);
        }
    }
}
