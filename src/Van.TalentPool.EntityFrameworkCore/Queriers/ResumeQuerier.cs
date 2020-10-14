using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Van.TalentPool.Application;
using Van.TalentPool.Application.Resumes;
using Van.TalentPool.Resumes;

namespace Van.TalentPool.EntityFrameworkCore.Queriers
{
    public class ResumeQuerier : IResumeQuerier
    {
        private readonly VanDbContext _context;
        public ResumeQuerier(VanDbContext context)
        {
            _context = context;
        }
        public async Task<PaginationOutput<ResumeDto>> GetListAsync(QueryResumeInput input)
        {
            var query = from a in _context.Resumes
                        join b in _context.Investigations on a.Id equals b.ResumeId into bb
                        from bbb in bb.DefaultIfEmpty()
                        join c in _context.Jobs on a.JobId equals c.Id
                        join d in _context.Users on a.CreatorUserId equals d.Id
                        join e in _context.Users on a.OwnerUserId equals e.Id
                        select new ResumeDto
                        {
                            Id = a.Id,
                            Name = a.Name,
                            JobId = a.JobId,
                            JobName = c.Title,
                            PhoneNumber = a.PhoneNumber,
                            CreationTime = a.CreationTime,
                            CreatorUserId = a.CreatorUserId,
                            CreatorUserName = d.FullName,
                            OwnerUserId = a.OwnerUserId,
                            OwnerUserName = e.FullName,
                            InvestigationId = bbb.Id,
                            AuditStatus = a.AuditStatus,
                            PlatformName = a.PlatformName,
                            PlatformId = a.PlatformId,
                            Enable = a.Enable
                        };

            if (!string.IsNullOrEmpty(input.Keyword))
                query = query.Where(w => w.Name.Contains(input.Keyword)
                || w.PhoneNumber.Contains(input.Keyword)
               || w.PlatformId.Contains(input.Keyword));
            if (input.JobId.HasValue)
                query = query.Where(w => w.JobId == input.JobId.Value);
            if (input.CreatorUserId.HasValue)
                query = query.Where(w => w.CreatorUserId == input.CreatorUserId.Value);
            if (input.OwnerUserId.HasValue)
                query = query.Where(w => w.OwnerUserId == input.OwnerUserId.Value);
            if (input.StartedTime.HasValue && input.EndedTime.HasValue)
                query = query.Where(w => w.CreationTime >= input.StartedTime.Value && w.CreationTime <= input.EndedTime.Value);
            if (input.AuditStatus.HasValue)
                query = query.Where(w => w.AuditStatus == (AuditStatus)input.AuditStatus.Value);

            var totalCount = await query.CountAsync();
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)input.PageSize);
            var resumes = await query.OrderByDescending(o => o.CreationTime)
                 .Skip((input.PageIndex - 1) * input.PageSize)
                .Take(input.PageSize)
                 .ToListAsync();
            return new PaginationOutput<ResumeDto>(totalSize, resumes);
        }

        public async Task<ResumeDetailDto> GetResumeAsync(Guid id)
        {
            var query = from a in _context.Resumes
                        join b in _context.Investigations on a.Id equals b.ResumeId into bb
                        from bbb in bb.DefaultIfEmpty()
                        join c in _context.Jobs on a.JobId equals c.Id
                        join d in _context.Users on a.CreatorUserId equals d.Id
                        join e in _context.Users on a.OwnerUserId equals e.Id
                        join f in _context.Users on a.OwnerUserId equals f.Id
                        select new ResumeDetailDto
                        {
                            Id = a.Id,
                            Name = a.Name,
                            JobId = a.JobId,
                            JobName = c.Title,
                            PhoneNumber = a.PhoneNumber,
                            CreationTime = a.CreationTime,
                            CreatorUserId = a.CreatorUserId,
                            CreatorUserName = d.FullName,
                            OwnerUserId = a.OwnerUserId,
                            OwnerUserName = e.FullName,
                            InvestigationId = bbb.Id,
                            AuditStatus = a.AuditStatus,
                            PlatformName = a.PlatformName,
                            PlatformId = a.PlatformId,
                            Enable = a.Enable,
                            EnableReason = a.EnableReason,
                            City = a.City,
                            Email = a.Email,
                            Description = a.Description,
                            LastModificationTime = a.LastModificationTime,
                            LastModifierUserName = e.FullName
                        };

            return await query.FirstOrDefaultAsync();

        }
    }
}
