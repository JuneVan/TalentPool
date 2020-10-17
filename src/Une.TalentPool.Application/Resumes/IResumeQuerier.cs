using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Une.TalentPool.Resumes;

namespace Une.TalentPool.Application.Resumes
{
    public interface IResumeQuerier
    {
        // 分页列表
        Task<PaginationOutput<ResumeDto>> GetListAsync(QueryResumeInput input);
        // 详情
        Task<ResumeDetailDto> GetResumeAsync(Guid id);
        Task<List<ResumeAuditRecordDto>> GetResumeAuditRecordsAsync(Guid resumeId);
        // 指定时间的简历统计
        Task<List<StatisticResumeDto>> GetStatisticResumesAsync(DateTime startTime, DateTime endTime, AuditStatus? auditStatus);
        Task<List<UncompleteResumeDto>> GetUncompleteResumesAsync(Guid? ownerUserId);
        // 导出数据
        Task<List<ResumeExportDto>> GetExportResumesAsync(QueryExportResumeInput input);
    }
}
