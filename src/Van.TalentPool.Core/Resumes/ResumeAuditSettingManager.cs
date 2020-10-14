using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Van.TalentPool.Resumes
{
    public class ResumeAuditSettingManager : IDisposable
    {
        protected bool _disposed;
        public ResumeAuditSettingManager(IResumeAuditSettingStore auditSettingStore)
        {
            AuditSettingStore = auditSettingStore;
        }
        protected IResumeAuditSettingStore AuditSettingStore { get; }
        protected CancellationToken CancellationToken => CancellationToken.None;

        public async Task<List<ResumeAuditSetting>> GetAuditSettingsAsync()
        {
            return await AuditSettingStore.GetAuditSettingsAsync(CancellationToken);
        }
        public async Task SaveAuditSettingsAsync(List<ResumeAuditSetting> auditSettings)
        {
            if (auditSettings == null)
                throw new ArgumentNullException(nameof(auditSettings));

            // 移除原来的记录
            var oldAuditSettings = await GetAuditSettingsAsync();
            if (oldAuditSettings != null)
                await AuditSettingStore.RemoveAuditSettingsAsync(oldAuditSettings, CancellationToken);
            // 提交新的记录
            await AuditSettingStore.AddAuditSettingsAsync(auditSettings, CancellationToken);
           await AuditSettingStore.CommitAsync(CancellationToken);
        }
       

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                AuditSettingStore.Dispose();
            }
            _disposed = true;
        }
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
