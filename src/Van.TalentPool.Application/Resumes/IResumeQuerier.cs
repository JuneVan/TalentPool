using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Van.TalentPool.Application.Resumes
{
    public interface IResumeQuerier
    {
        Task<PaginationOutput<ResumeDto>> GetListAsync(QueryResumeInput input);
        Task<ResumeDetailDto> GetResumeAsync(Guid id);
        Task<List<ResumeAuditRecordDto>> GetResumeAuditRecordsAsync(Guid resumeId);

        Task<List<MonthlyResumeDto>> GetMonthlyResumesAsync(DateTime startTime, DateTime endTime);
        Task<List<UncompleteResumeDto>> GetUncompleteResumesAsync(Guid? ownerUserId);
    }
}
