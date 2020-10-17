using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Une.TalentPool.Application;
using Une.TalentPool.Application.Interviews;
using Une.TalentPool.Interviews;

namespace Une.TalentPool.EntityFrameworkCore.Queriers
{
    public class InterviewQuerier : IInterviewQuerier
    {
        private readonly VanDbContext _context;
        public InterviewQuerier(VanDbContext context)
        {
            _context = context;
        }
        public async Task<PaginationOutput<InterviewDto>> GetListAsync(QueryInterviewInput input)
        {
            var query = from a in _context.Interviews
                        join b in _context.Resumes on a.ResumeId equals b.Id
                        join c in _context.Jobs on b.JobId equals c.Id
                        join d in _context.Users on a.CreatorUserId equals d.Id
                        select new InterviewDto()
                        {
                            Id = a.Id,
                            Name = b.Name,
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
            var totalCount = await query.CountAsync();
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)input.PageSize);
            var interviews = await query.OrderByDescending(o => o.CreationTime)
                 .Skip((input.PageIndex - 1) * input.PageSize)
                .Take(input.PageSize)
                 .ToListAsync();
            return new PaginationOutput<InterviewDto>(totalSize, interviews);
        }

        public async Task<List<StatisticInterviewDto>> GetStatisticInterviewsAsync(DateTime startTime, DateTime endTime)
        {
            var query = from a in _context.Interviews
                        where a.CreationTime >= startTime && a.CreationTime <= endTime
                        select new StatisticInterviewDto()
                        {
                            CreatorUserId = a.CreatorUserId
                        };
            return await query.ToListAsync();
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
