﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Van.TalentPool.Resumes;

namespace Van.TalentPool.Application.Resumes
{
    public interface IResumeQuerier
    {
        // 分页列表
        Task<PaginationOutput<ResumeDto>> GetListAsync(QueryResumeInput input);
        // 详情
        Task<ResumeDetailDto> GetResumeAsync(Guid id);
        Task<List<ResumeAuditRecordDto>> GetResumeAuditRecordsAsync(Guid resumeId);

        Task<List<StatisticResumeDto>> GetStatisticResumesAsync(DateTime startTime, DateTime endTime, AuditStatus? auditStatus);
        Task<List<UncompleteResumeDto>> GetUncompleteResumesAsync(Guid? ownerUserId);
        // 导出数据
        Task<List<ResumeExportDto>> GetExportResumesAsync(QueryExportResumeInput input);
    }
}
