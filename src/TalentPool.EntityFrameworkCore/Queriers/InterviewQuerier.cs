using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TalentPool.Application;
using TalentPool.Application.Interviews;
using TalentPool.Interviews;

namespace TalentPool.EntityFrameworkCore.Queriers
{
    public class InterviewQuerier : IInterviewQuerier
    {
        private readonly TalentDbContext _context;
        private readonly ISignal _signal;
        public InterviewQuerier(TalentDbContext context, ISignal signal)
        {
            _context = context;
            _signal = signal;
        }
        protected CancellationToken CancellationToken => _signal.Token;
        public async Task<List<InterviewCalendarDto>> GetCalendarInterviewsAsync(DateTime startTime, DateTime endTime)
        {
            CancellationToken.ThrowIfCancellationRequested();
            if (startTime == null)
                throw new ArgumentNullException(nameof(startTime));
            if (endTime == null)
                throw new ArgumentNullException(nameof(endTime));

            var query = from a in _context.Interviews
                        join b in _context.Resumes on a.ResumeId equals b.Id
                        join c in _context.Jobs on b.JobId equals c.Id
                        where a.AppointmentTime >= startTime && a.AppointmentTime <= endTime
                        select new InterviewCalendarDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            AppointmentTime = a.AppointmentTime,
                            Status = a.Status,
                            JobName = c.Title
                        };
            return await query.ToListAsync(CancellationToken);
        }

        public async Task<InterviewDetailDto> GetInterviewDetailAsync(Guid id)
        {
            CancellationToken.ThrowIfCancellationRequested();
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var query = from a in _context.Interviews
                        join b in _context.Resumes on a.ResumeId equals b.Id
                        join c in _context.Jobs on b.JobId equals c.Id
                        join d in _context.Users on a.CreatorUserId equals d.Id
                        join e in _context.Users on a.LastModifierUserId equals e.Id into ee
                        from eee in ee.DefaultIfEmpty()
                        join f in _context.Investigations on a.ResumeId equals f.ResumeId into ff
                        from fff in ff.DefaultIfEmpty()
                        where a.Id == id
                        select new InterviewDetailDto()
                        {
                            Name = a.Name,
                            AppointmentTime = a.AppointmentTime,
                            CreationTime = a.CreationTime,
                            CreatorUserName = d.FullName,
                            JobName = c.Title,
                            LastModificationTime = a.LastModificationTime,
                            LastModifierUserName = eee == null ? string.Empty : eee.FullName,
                            Remark = a.Remark,
                            Status = a.Status,
                            VisitedTime = a.VisitedTime,
                            ResumeId = a.ResumeId,
                            InvestigationId = fff == null ? (Guid?)null : fff.Id
                        };

            return await query.FirstOrDefaultAsync(CancellationToken);
        }

        public async Task<PaginationOutput<InterviewDto>> GetListAsync(QueryInterviewInput input)
        {
            CancellationToken.ThrowIfCancellationRequested();
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var query = from a in _context.Interviews
                        join b in _context.Resumes on a.ResumeId equals b.Id
                        join c in _context.Jobs on b.JobId equals c.Id
                        join d in _context.Users on a.CreatorUserId equals d.Id
                        join f in _context.Users on b.OwnerUserId equals f.Id
                        select new InterviewDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            AppointmentTime = a.AppointmentTime,
                            Status = a.Status,
                            JobName = c.Title,
                            CreatorUserId = a.CreatorUserId,
                            CreatorUserName = d.FullName,
                            OwnerUserId = b.OwnerUserId,
                            OwnerUserName = f.FullName,
                            PhoneNumber = b.PhoneNumber,
                            Remark = a.Remark,
                            VisitedTime = a.VisitedTime,
                            CreationTime = a.CreationTime
                        };
            if (!string.IsNullOrEmpty(input.Keyword))
                query = query.Where(w => w.Name.Contains(input.Keyword));
            if (input.StartTime.HasValue && input.EndTime.HasValue)
                query = query.Where(w => w.AppointmentTime >= input.StartTime && w.AppointmentTime <= input.EndTime);
            if (input.CreatorUserId.HasValue && input.CreatorUserId.Value != Guid.Empty)
                query = query.Where(w => w.CreatorUserId == input.CreatorUserId);
            if (input.Status.HasValue)
                query = query.Where(w => w.Status == (InterviewStatus)input.Status.Value);
            var totalCount = await query.CountAsync(CancellationToken);
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)input.PageSize);
            var interviews = await query.OrderByDescending(o => o.CreationTime)
                 .Skip((input.PageIndex - 1) * input.PageSize)
                .Take(input.PageSize)
                 .ToListAsync(CancellationToken);
            return new PaginationOutput<InterviewDto>(totalSize, interviews);
        }

        public async Task<List<InterviewStatisticDto>> GetStatisticInterviewsAsync(DateTime startTime, DateTime endTime)
        {
            CancellationToken.ThrowIfCancellationRequested();
            if (startTime == null)
                throw new ArgumentNullException(nameof(startTime));
            if (endTime == null)
                throw new ArgumentNullException(nameof(endTime));

            var query = from a in _context.Interviews
                        where a.CreationTime >= startTime && a.CreationTime <= endTime
                        select new InterviewStatisticDto()
                        {
                            CreatorUserId = a.CreatorUserId
                        };
            return await query.ToListAsync(CancellationToken);
        }

        public async Task<List<ReportInterviewDto>> GetReportInterviewsAsync(DateTime date)
        {
            var query = from a in _context.Interviews
                        join b in _context.Resumes on a.ResumeId equals b.Id
                        join d in _context.Jobs on b.JobId equals d.Id
                        orderby a.AppointmentTime
                        select new ReportInterviewDto()
                        {
                            Name = a.Name,
                            AppointmentTime = a.AppointmentTime,
                            JobName = d.Title,
                            Remark = a.Remark,
                            Status = a.Status,
                            VisitedTime = a.VisitedTime
                        };
            query = query.Where(w => w.AppointmentTime.Date == date.Date);
            return await query.ToListAsync();
        }

        public async Task<List<UnfinshInterviewDto>> GetUnfinshInterviewsAsync(Guid? ownerUserId)
        {
            var query = from a in _context.Interviews
                        join b in _context.Resumes on a.ResumeId equals b.Id
                        join d in _context.Jobs on b.JobId equals d.Id
                        join e in _context.Users on a.CreatorUserId equals e.Id
                        join f in _context.Users on b.OwnerUserId equals f.Id
                        orderby a.AppointmentTime
                        where a.Status == InterviewStatus.None
                        select new UnfinshInterviewDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            AppointmentTime = a.AppointmentTime,
                            JobName = d.Title,
                            PhoneNumber = b.PhoneNumber,
                            CreatorUserId = a.CreatorUserId,
                            CreatorUserName = e.FullName,
                            OwnerUserId = b.OwnerUserId,
                            OwnerUserName = f.FullName,
                            Status = a.Status,
                            CreationTime = a.CreationTime
                        };
            if (ownerUserId.HasValue)
                query = query.Where(w => w.OwnerUserId == ownerUserId);
            return await query.ToListAsync();
        }
    }
}
