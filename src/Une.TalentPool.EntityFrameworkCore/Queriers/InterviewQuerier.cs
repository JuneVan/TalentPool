using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Une.TalentPool.Application;
using Une.TalentPool.Application.Interviews;
using Une.TalentPool.Interviews;

namespace Une.TalentPool.EntityFrameworkCore.Queriers
{
    public class InterviewQuerier : IInterviewQuerier
    {
        private readonly TalentDbContext _context;
        private readonly ICancellationTokenProvider _tokenProvider;
        public InterviewQuerier(TalentDbContext context, ICancellationTokenProvider tokenProvider)
        {
            _context = context;
            _tokenProvider = tokenProvider;
        }
        protected CancellationToken CancellationToken => _tokenProvider.Token;
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
                            VisitedTime = a.VisitedTime
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
                        select new InterviewDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            AppointmentTime = a.AppointmentTime,
                            Status = a.Status,
                            JobName = c.Title,
                            CreatorUserId = a.CreatorUserId,
                            CreatorUserName = d.FullName,
                            PhoneNumber = b.PhoneNumber,
                            Remark = a.Remark,
                            VisitedTime = a.VisitedTime,
                            CreationTime = a.CreationTime
                        };
            if (!string.IsNullOrEmpty(input.Keyword))
                query = query.Where(w => w.Name.Contains(input.Keyword));
            if (input.StartTime.HasValue && input.EndTime.HasValue)
                query = query.Where(w => w.AppointmentTime >= input.StartTime && w.AppointmentTime <= input.EndTime);
            if (input.CreatorUserId.HasValue)
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

        public async Task<List<UnfinshInterviewDto>> GetUnfinshInterviewsAsync(Guid? creatorUserId)
        {
            var query = from a in _context.Interviews
                        join b in _context.Resumes on a.ResumeId equals b.Id
                        join d in _context.Jobs on b.JobId equals d.Id
                        join e in _context.Users on a.CreatorUserId equals e.Id
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
                            Status = a.Status
                        };
            if (creatorUserId.HasValue)
                query = query.Where(w => w.CreatorUserId == creatorUserId);
            return await query.ToListAsync();
        }
    }
}
