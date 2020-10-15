using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Van.TalentPool.Application;
using Van.TalentPool.Application.Interviews;
using Van.TalentPool.Interviews;

namespace Van.TalentPool.EntityFrameworkCore.Queriers
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
    }
}
