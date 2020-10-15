using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Van.TalentPool.Resumes
{
    public class ResumeManager : IDisposable
    {
        private bool _disposed; 
        public ResumeManager(IResumeStore resumeStore,
            IResumeAuditSettingStore resumeAuditSettingStore,
            IEnumerable<IResumeValidator> resumeValidators,
            IResumeComparer resumeComparer )
        {
            ResumeStore = resumeStore;
            ResumeValidators = resumeValidators;
            ResumeComparer = resumeComparer;
            ResumeAuditSettingStore = resumeAuditSettingStore; 
        }
        protected IResumeStore ResumeStore { get; }
        protected IResumeAuditSettingStore ResumeAuditSettingStore { get; }
        protected IEnumerable<IResumeValidator> ResumeValidators { get; }
        protected IResumeComparer ResumeComparer { get; }
        protected virtual CancellationToken CancellationToken => CancellationToken.None;

        public async Task<Resume> CreateAsync(Resume resume)
        {
            if (resume == null)
                throw new ArgumentNullException(nameof(resume));
            await ValidateAsync(resume);
            return await ResumeStore.CreateAsync(resume, CancellationToken);
        }

        public async Task<Resume> UpdateAsync(Resume resume)
        {
            if (resume == null)
                throw new ArgumentNullException(nameof(resume));
            // 重复性验证
            await ValidateAsync(resume);
            //获取相似简历
            await CompareAsync(resume);
            return await ResumeStore.UpdateAsync(resume, CancellationToken);
        }

        private async Task ValidateAsync(Resume resume)
        {
            if (ResumeValidators != null)
            {
                foreach (var validator in ResumeValidators)
                {
                    await validator.ValidateAsync(this, resume);
                }
            }
        }
        private async Task CompareAsync(Resume resume)
        {
            if (ResumeComparer != null)
            {
                resume.ResumeCompares = await ResumeComparer.CompareAsync(this, resume);
            }
        }
        public async Task DeleteAsync(Resume resume)
        {
            if (resume == null)
                throw new ArgumentNullException(nameof(resume));
            await ResumeStore.DeleteAsync(resume, CancellationToken);
        }
        public async Task<Resume> FindByIdAsync(Guid resumeId)
        {
            if (resumeId == null)
                throw new ArgumentNullException(nameof(resumeId));

            return await ResumeStore.FindByIdAsync(resumeId, CancellationToken);
        }
        public async Task<Resume> FindByPhoneNumberAsync(string phoneNumber)
        {
            if (phoneNumber == null)
                throw new ArgumentNullException(nameof(phoneNumber));

            return await ResumeStore.FindByPhoneNumberAsync(phoneNumber, CancellationToken);
        }
        public async Task<Resume> FindByPlatformAsync(string platformId)
        {
            if (platformId == null)
                throw new ArgumentNullException(nameof(platformId));
            return await ResumeStore.FindByPlatformAsync(platformId, CancellationToken);
        }

       
        public async Task<ResumeAuditRecord> GetAuditRecordByIdAsync(Guid auditRecordId)
        {
            if (auditRecordId == null)
                throw new ArgumentNullException(nameof(auditRecordId));

            return await ResumeStore.GetAuditRecordByIdAsync(auditRecordId, CancellationToken);
        }

        public async Task AuditAsync(Resume resume, bool passed, Guid auditedUserId, ResumeAuditRecord auditRecord)
        {
            if (resume == null)
                throw new ArgumentNullException(nameof(resume));

            if (resume.AuditStatus == AuditStatus.Complete || resume.AuditStatus == AuditStatus.Unpassed)
                throw new InvalidOperationException("当前简历已处理，请勿重复操作。");

            // 检查审批进度
            var auditSettings = await ResumeAuditSettingStore.GetAuditSettingsAsync(CancellationToken);

            // 检查当前用户符合审批条件
            var auditSetting = auditSettings.FirstOrDefault(f => f.UserId == auditedUserId);
            if (auditSetting == null)
                throw new InvalidOperationException("当前用户无审核权限。");
            // 设置审核状态
            // 如果当前节点序号大于或等于整个审批人总数，则表示审批结束
            AuditStatus auditStatus = AuditStatus.Ongoing;
            if (auditSetting.Order >= auditSettings.Count - 1)
                auditStatus = AuditStatus.Complete;
            if (!passed)
                auditStatus = AuditStatus.Unpassed;

            var audit = await ResumeStore.AddAuditRecordAsync(resume, auditRecord, CancellationToken);
            resume.AuditStatus = auditStatus;
            await ResumeStore.UpdateAsync(resume, CancellationToken);
        }

        public async Task CancelAuditAsync(Resume resume, Guid auditedUserId, ResumeAuditRecord auditRecord)
        {

            if (resume == null)
                throw new ArgumentNullException(nameof(resume));
            if (auditedUserId == null)
                throw new ArgumentNullException(nameof(auditedUserId));
            if (auditRecord == null)
                throw new ArgumentNullException(nameof(auditRecord));

            if (auditRecord.CreatorUserId != auditedUserId)
                throw new InvalidOperationException("当前审核记录非当前用户所有，无法撤销操作。");

            await ResumeStore.RemoveAuditRecordAsync(resume, auditRecord, CancellationToken);

            resume.AuditStatus = AuditStatus.Ongoing;
            await ResumeStore.UpdateAsync(resume, CancellationToken);
        }

        public async Task AssignUserAsync(Resume resume, Guid ownerUserId)
        {
            if (resume == null)
                throw new ArgumentNullException(nameof(resume));
            if (ownerUserId == null)
                throw new ArgumentNullException(nameof(ownerUserId));


            resume.OwnerUserId = ownerUserId;

            await ResumeStore.UpdateAsync(resume, CancellationToken);
            
        }

        public async Task TrashAsync(Resume resume,string enableReason)
        {
            if (resume == null)
                throw new ArgumentNullException(nameof(resume));
            if (string.IsNullOrEmpty(enableReason))
                throw new ArgumentNullException(nameof(enableReason)); 
            resume.Enable = false;
            resume.EnableReason = enableReason; 
            await ResumeStore.UpdateAsync(resume, CancellationToken);

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
                ResumeStore.Dispose();
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
