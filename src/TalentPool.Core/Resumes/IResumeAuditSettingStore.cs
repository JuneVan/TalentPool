using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TalentPool.Resumes
{
    public interface IResumeAuditSettingStore : IDisposable
    {
        Task<List<ResumeAuditSetting>> GetAuditSettingsAsync(CancellationToken cancellationToken);
        Task AddAuditSettingsAsync(List<ResumeAuditSetting> settings, CancellationToken cancellationToken);
        Task RemoveAuditSettingsAsync(List<ResumeAuditSetting> settings, CancellationToken cancellationToken); 
        Task CommitAsync(CancellationToken cancellationToken);
    }
}
