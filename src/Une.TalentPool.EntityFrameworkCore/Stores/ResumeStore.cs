using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Une.TalentPool.Resumes;

namespace Une.TalentPool.EntityFrameworkCore.Stores
{
    public class ResumeStore : StoreBase, IResumeStore
    {
        public ResumeStore(VanDbContext context) : base(context)
        {

        }
        public async Task<Resume> CreateAsync(Resume resume, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (resume == null)
                throw new ArgumentNullException(nameof(resume));
            Context.Resumes.Add(resume);
            await SaveChanges(cancellationToken);
            return resume;
        }

        public async Task<Resume> UpdateAsync(Resume resume, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (resume == null)
                throw new ArgumentNullException(nameof(resume));

            // 清除子实体集合防止更新异常
            var keyMaps = await Context.ResumeKeyMaps.Where(w => w.ResumeId == resume.Id).ToListAsync();
            if (keyMaps != null)
            {
                foreach (var keymap in keyMaps)
                {
                    Context.ResumeKeyMaps.Remove(keymap);
                }
            }
            var compares = await Context.ResumeCompares.Where(w => w.ResumeId == resume.Id).ToListAsync();
            if (compares != null)
            {
                foreach (var compare in compares)
                {
                    Context.ResumeCompares.Remove(compare);
                }
            }

            Context.Resumes.Attach(resume);
            resume.ConcurrencyStamp = Guid.NewGuid().ToString();
            Context.Resumes.Update(resume);
            try
            {
                await SaveChanges(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException("Optimistic concurrency failure, object has been modified.");
            }
            return resume;
        }
        public async Task<Resume> DeleteAsync(Resume resume, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (resume == null)
                throw new ArgumentNullException(nameof(resume));
            Context.Resumes.Remove(resume);
            await SaveChanges(cancellationToken);
            return resume;
        }

        public async Task<Resume> FindByIdAsync(Guid resumeId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (resumeId == null)
                throw new ArgumentNullException(nameof(resumeId));
            return await Context.Resumes.Include(i => i.KeyMaps)
                .Include(i => i.ResumeCompares) 
                .FirstOrDefaultAsync(f => f.Id == resumeId, cancellationToken);

        }
        public async Task<Resume> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (phoneNumber == null)
                throw new ArgumentNullException(nameof(phoneNumber));
            return await Context.Resumes.FirstOrDefaultAsync(f => f.PhoneNumber == phoneNumber, cancellationToken);
        }

        public async Task<Resume> FindByPlatformAsync(string platformId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (platformId == null)
                throw new ArgumentNullException(nameof(platformId));
            return await Context.Resumes.FirstOrDefaultAsync(f => f.PlatformId == platformId, cancellationToken);
        }


        public async Task<ResumeAuditRecord> GetAuditRecordByIdAsync(Guid auditRecordId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (auditRecordId == null)
                throw new ArgumentNullException(nameof(auditRecordId));
            return await Context.ResumeAuditRecords.FirstOrDefaultAsync(f => f.Id == auditRecordId);
        }

        public async Task<ResumeAuditRecord> AddAuditRecordAsync(Resume resume, ResumeAuditRecord auditRecord, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (resume == null)
                throw new ArgumentNullException(nameof(resume));
            if (auditRecord == null)
                throw new ArgumentNullException(nameof(auditRecord));
            auditRecord.ResumeId = resume.Id;
            Context.ResumeAuditRecords.Add(auditRecord);
            return await Task.FromResult(auditRecord);
        }

        public async Task<ResumeAuditRecord> RemoveAuditRecordAsync(Resume resume, ResumeAuditRecord auditRecord, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (resume == null)
                throw new ArgumentNullException(nameof(resume));
            if (auditRecord == null)
                throw new ArgumentNullException(nameof(auditRecord));
            Context.ResumeAuditRecords.Remove(auditRecord);
            return await Task.FromResult(auditRecord);
        }
    }
}
