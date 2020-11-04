using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TalentPool.Application;
using TalentPool.Application.Resumes;
using TalentPool.Resumes;

namespace TalentPool.Dapper.Queriers
{
    public class ResumeQuerier : IResumeQuerier
    {
        private readonly IConnectionProvider _provider;
        public ResumeQuerier(IConnectionProvider provider)
        {
            _provider = provider;
        }


        public async Task<PaginationOutput<ResumeDto>> GetListAsync(QueryResumeInput input)
        {
            using (var connection = await _provider.GetDbConnectionAsync())
            {
                string columnSql = "a.Id,a.Name,a.JobId,c.Title,a.PhoneNumber," +
                    "a.CreationTime,a.CreatorUserId,CONCAT(d.Surname,d.Name) CreatorUserName ,a.OwnerUserId,CONCAT(e.Surname,e.Name) OwnerUserName," +
                    "b.Id as InvestigationId,a.AuditStatus,a.PlatformName,a.PlatformId," +
                    "a.Enable,a.EnableReason";

                var fromSql = new StringBuilder();
                fromSql.Append("from resumes a ");
                fromSql.Append("left join investigations b on a.Id = b.ResumeId ");
                fromSql.Append("inner join jobs c on a.JobId = c.Id ");
                fromSql.Append("inner join users d on a.CreatorUserId = d.Id ");
                fromSql.Append("inner join users e on a.OwnerUserId = e.id ");
                fromSql.Append(" where 1 = 1");
                if (!string.IsNullOrEmpty(input.Keyword))
                    fromSql.Append(" and (a.Name like '%@Keyword%' or a.PhoneNumber like '%@Keyword%' or a.PlatformId like '%@Keyword%') ");
                if (input.JobId.HasValue)
                    fromSql.Append(" and a.JobId = @JobId ");
                if (input.CreatorUserId.HasValue)
                    fromSql.Append(" and a.CreatorUserId = @CreatorUserId ");
                if (input.OwnerUserId.HasValue && input.OwnerUserId != Guid.Empty)
                    fromSql.Append(" and a.OwnerUserId = @OwnerUserId ");
                if (input.StartTime.HasValue && input.EndTime.HasValue)
                    fromSql.Append(" and a.CreationTime >= @StartTime and a.CreationTime<= @EndTime ");
                if (input.AuditStatus.HasValue)
                    fromSql.Append(" and a.AuditStatus = @AuditStatus ");

                return await connection.GetPaginationAsync<ResumeDto>(columnSql, fromSql.ToString(), input, input.PageSize, input.PageIndex);
            }

        }


        public async Task<ResumeDetailDto> GetResumeAsync(Guid id)
        {
            return await Task.FromResult<ResumeDetailDto>(null);
        }

        public async Task<List<ResumeAuditRecordDto>> GetResumeAuditRecordsAsync(Guid resumeId)
        {
            if (resumeId == null)
                throw new ArgumentNullException(nameof(resumeId));
            return await Task.FromResult<List<ResumeAuditRecordDto>>(null);
        }

        public async Task<List<StatisticResumeDto>> GetStatisticResumesAsync(DateTime startTime, DateTime endTime, AuditStatus? auditStatus)
        {
            return await Task.FromResult<List<StatisticResumeDto>>(null);
        }

        public async Task<List<UncompleteResumeDto>> GetUncompleteResumesAsync(Guid? ownerUserId)
        {
            return await Task.FromResult<List<UncompleteResumeDto>>(null);
        }


        public async Task<List<ResumeExportDto>> GetExportResumesAsync(QueryExportResumeInput input)
        {
            return await Task.FromResult<List<ResumeExportDto>>(null);
        }
    }
}
