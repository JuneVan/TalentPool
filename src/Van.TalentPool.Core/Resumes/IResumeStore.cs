using System;
using System.Threading;
using System.Threading.Tasks;

namespace Van.TalentPool.Resumes
{
    public interface IResumeStore : IDisposable
    {
        Task<Resume> FindByIdAsync(Guid resumeId, CancellationToken cancellationToken);
        Task<Resume> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);
        Task<Resume> FindByPlatformAsync(string platformId, CancellationToken cancellationToken);
        Task<Resume> CreateAsync(Resume resume, CancellationToken cancellationToken);
        Task<Resume> UpdateAsync(Resume resume, CancellationToken cancellationToken);
        Task<Resume> DeleteAsync(Resume resume, CancellationToken cancellationToken);
        Task<ResumeAuditRecord> GetAuditRecordByIdAsync(Guid auditRecordId, CancellationToken cancellationToken);
        Task<ResumeAuditRecord> AddAuditRecordAsync(Resume resume, ResumeAuditRecord auditRecord, CancellationToken cancellationToken);
        Task<ResumeAuditRecord> RemoveAuditRecordAsync(Resume resume, ResumeAuditRecord auditRecord, CancellationToken cancellationToken);

        //Task<List<ResumeKeywordMap>> GetResumeKeyMapsAsync(string keyword, CancellationToken cancellationToken);
    }
}
