using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Van.TalentPool.Application.Interviews
{
    public interface IInterviewQuerier
    {
        Task<PaginationOutput<InterviewDto>> GetListAsync(QueryInterviewInput input);

        Task<List<InterviewMonthyDto>> GetMonthyInterviewsAsync(DateTime startTime, DateTime endTime);

        Task<List<UnfinshInterviewDto>> GetUnfinshInterviewsAsync(Guid?creatorUserId);
    }
}
