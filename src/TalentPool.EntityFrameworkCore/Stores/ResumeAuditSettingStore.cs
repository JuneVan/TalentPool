using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TalentPool.Resumes;

namespace TalentPool.EntityFrameworkCore.Stores
{
    public class ResumeAuditSettingStore : StoreBase, IResumeAuditSettingStore
    {
        public ResumeAuditSettingStore(TalentDbContext context) : base(context)
        {

        }


        public async Task<List<ResumeAuditSetting>> GetAuditSettingsAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return await Context.ResumeAuditSettings.ToListAsync();
        }
        public async Task AddAuditSettingsAsync(List<ResumeAuditSetting> settings, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            Context.ResumeAuditSettings.AddRange(settings);
            await Task.CompletedTask;
        }
        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await SaveChanges(cancellationToken);
        }

        public async Task RemoveAuditSettingsAsync(List<ResumeAuditSetting> settings, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            Context.ResumeAuditSettings.RemoveRange(settings);
            await Task.CompletedTask;
        }


    }
}
