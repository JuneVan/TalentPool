using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Van.TalentPool.Resumes;

namespace Van.TalentPool.Application.Resumes
{
    public interface IResumeQuerier
    {
        Task<PaginationOutput<ResumeDto>> GetListAsync(QueryResumeInput input);
        Task<ResumeDetailDto> GetResumeAsync(Guid id);
        Task<List<ResumeAuditRecordDto>> GetResumeAuditRecordsAsync(Guid resumeId);

        Task<List<StatisticResumeDto>> GetStatisticResumesAsync(DateTime startTime, DateTime endTime, AuditStatus? auditStatus);
        Task<List<UncompleteResumeDto>> GetUncompleteResumesAsync(Guid? ownerUserId);
    }
}
