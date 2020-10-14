using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Van.TalentPool.Application;
using Van.TalentPool.Application.Investigations;
using Van.TalentPool.Investigations;

namespace Van.TalentPool.EntityFrameworkCore.Queriers
{
    public class InvestigationQuerier : IInvestigationQuerier
    {
        private readonly VanDbContext _context;
        public InvestigationQuerier(VanDbContext context)
        {
            _context = context;
        }

        public async Task<PaginationOutput<InvestigationDto>> GetListAsync(QueryInvestigaionInput input)
        {
            var query = from a in _context.Investigations
                        join b in _context.Resumes on a.ResumeId equals b.Id
                        join c in _context.Jobs on b.JobId equals c.Id
                        join d in _context.Users on b.OwnerUserId equals d.Id
                        join e in _context.Users on b.CreatorUserId equals e.Id
                        select new InvestigationDto()
                        {
                            Name = a.Name,
                            Id = a.Id,
                            JobId = b.JobId,
                            JobName = c.Title,
                            ResumeId = a.ResumeId,
                            CreationTime = a.CreationTime,
                            PhoneNumber = b.PhoneNumber,
                            Status = a.Status,
                            IsQualified = a.IsQualified,
                            OwnerUserId = b.OwnerUserId,
                            OwnerUserName = d.FullName,
                            InvestigateDate = a.InvestigateDate,
                            AcceptTravelStatus = a.AcceptTravelStatus
                        };
            if (!string.IsNullOrEmpty(input.Keyword))
                query = query.Where(w => w.Name.Contains(input.Keyword)
               || w.PhoneNumber.Contains(input.Keyword));
            if (input.JobId.HasValue)
                query = query.Where(w => w.JobId == input.JobId);
            if (input.OwnerUserId.HasValue && input.OwnerUserId != Guid.Empty)
                query = query.Where(w => w.OwnerUserId == input.OwnerUserId);
            if (input.Status.HasValue)
                query = query.Where(w => w.Status == (InvestigationStatus)input.Status.Value);
            if (input.StartTime.HasValue && input.EndTime.HasValue)
                query = query.Where(w => w.InvestigateDate >= input.StartTime.Value);


            var totalCount = await query.CountAsync();
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)input.PageSize);
            var investigations = await query.OrderByDescending(o => o.CreationTime)
                 .Skip((input.PageIndex - 1) * input.PageSize)
                .Take(input.PageSize)
                 .ToListAsync();
            return new PaginationOutput<InvestigationDto>(totalSize, investigations);
        }
    }
}
