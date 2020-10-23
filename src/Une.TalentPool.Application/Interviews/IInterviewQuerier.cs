using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Une.TalentPool.Application.Interviews
{
    public interface IInterviewQuerier
    {
        Task<PaginationOutput<InterviewDto>> GetListAsync(QueryInterviewInput input);

        Task<List<InterviewStatisticDto>> GetStatisticInterviewsAsync(DateTime startTime, DateTime endTime);

        Task<List<UnfinshInterviewDto>> GetUnfinshInterviewsAsync(Guid?creatorUserId);

        Task<List<InterviewCalendarDto>> GetCalendarInterviewsAsync(DateTime startTime, DateTime endTime);
        Task<InterviewDetailDto> GetInterviewDetailAsync(Guid id);
    }
}
