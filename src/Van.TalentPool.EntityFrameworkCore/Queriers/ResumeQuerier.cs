using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Van.TalentPool.Application;
using Van.TalentPool.Application.Resumes;
using Van.TalentPool.Investigations;
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
                            Enable = a.Enable,
                            EnableReason = a.EnableReason
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
            if (input.StartTime.HasValue && input.EndTime.HasValue)
                query = query.Where(w => w.CreationTime >= input.StartTime.Value && w.CreationTime <= input.EndTime.Value);
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

            var resume = await query.FirstOrDefaultAsync();
            resume.ResumeAuditRecords = await GetResumeAuditRecordsAsync(id);
            return resume;

        }

        public async Task<List<ResumeAuditRecordDto>> GetResumeAuditRecordsAsync(Guid resumeId)
        {
            if (resumeId == null)
                throw new ArgumentNullException(nameof(resumeId));
            var query = from a in _context.ResumeAuditRecords
                        join b in _context.Users on a.CreatorUserId equals b.Id
                        where a.ResumeId == resumeId
                        select new ResumeAuditRecordDto()
                        {

                            Id = a.Id,
                            CreatorUserId = a.CreatorUserId,
                            CreationTime = a.CreationTime,
                            CreatorUserName = b.FullName,
                            Passed = a.Passed,
                            Remark = a.Remark
                        };

            return await query.ToListAsync();
        }

        public async Task<List<MonthlyResumeDto>> GetMonthlyResumesAsync(DateTime startTime, DateTime endTime)
        {
            var query = from a in _context.Resumes
                        join b in _context.Users on a.CreatorUserId equals b.Id
                        where a.CreationTime >= startTime && a.CreationTime <= endTime
                        select new MonthlyResumeDto()
                        {
                            OwnerUserId = a.OwnerUserId,
                            CreatorUserId = a.CreatorUserId,
                            CreationTime = a.CreationTime,
                            CreatorUserName = b.FullName,
                            AuditStatus = a.AuditStatus,
                            CreatorUserPhoto = b.Photo
                        };

            return await query.ToListAsync();
        }

        public async Task<List<UncompleteResumeDto>> GetUncompleteResumesAsync(Guid? ownerUserId)
        {
            var query = from a in _context.Resumes
                        join b in _context.Investigations on a.Id equals b.ResumeId into bb
                        from bbb in bb.DefaultIfEmpty()
                        join c in _context.Jobs on a.JobId equals c.Id
                        join d in _context.Users on a.OwnerUserId equals d.Id
                        where a.AuditStatus == AuditStatus.Complete && (bbb == null || bbb.Status != InvestigationStatus.Complete)
                        orderby a.CreationTime
                        select new UncompleteResumeDto
                        {
                            Id = a.Id,
                            PlatformName = a.PlatformName,
                            PlatformId = a.PlatformId,
                            Name = a.Name,
                            JobName = c.Title,
                            PhoneNumber = a.PhoneNumber,
                            CreationTime = a.CreationTime,
                            OwnerUserId = a.OwnerUserId,
                            OwnerUserName = d.FullName,
                            InvestigationId = bbb == null ? (Guid?)null : bbb.Id,
                            Status = bbb == null ? (InvestigationStatus?)null : bbb.Status,
                            InvestigationDate = bbb == null ? (DateTime?)null : bbb.InvestigateDate,
                        };
            return await query.ToListAsync();
        }
    }
}
