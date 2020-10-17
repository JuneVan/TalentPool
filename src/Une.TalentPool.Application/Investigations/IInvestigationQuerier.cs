using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Une.TalentPool.Application.Investigations
{
    public interface IInvestigationQuerier
    {
        //分页列表
        Task<PaginationOutput<InvestigationDto>> GetListAsync(QueryInvestigaionInput input);
        //详情
        Task<InvestigationDetailDto> GetInvestigationAsync(Guid id);
        // 统计数据
        Task<List<StatisticInvestigationDto>> GetStatisticInvestigationsAsync(DateTime startTime, DateTime endTime);
        //每日报告
        Task<List<ReportInvestigationDto>> GetReportInvestigationsAsync(DateTime date);
    }
}
