using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TalentPool.Application.Interviews
{
    public interface IInterviewQuerier
    {
        Task<PaginationOutput<InterviewDto>> GetListAsync(QueryInterviewInput input);

        Task<List<InterviewStatisticDto>> GetStatisticInterviewsAsync(DateTime startTime, DateTime endTime);
        /// <summary>
        /// 获取未完成预约列表
        /// </summary>
        /// <param name="creatorUserId"></param>
        /// <returns></returns>
        Task<List<UnfinshInterviewDto>> GetUnfinshInterviewsAsync(Guid?creatorUserId);

        Task<List<InterviewCalendarDto>> GetCalendarInterviewsAsync(DateTime startTime, DateTime endTime);
        Task<InterviewDetailDto> GetInterviewDetailAsync(Guid id);
        /// <summary>
        /// 获取指定日期面试情况
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<List<ReportInterviewDto>> GetReportInterviewsAsync(DateTime date);
    }
}
