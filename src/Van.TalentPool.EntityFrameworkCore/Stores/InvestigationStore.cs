using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Van.TalentPool.Investigations;

namespace Van.TalentPool.EntityFrameworkCore.Stores
{
    public class InvestigationStore : StoreBase, IInvestigationStore
    {
        public InvestigationStore(VanDbContext context)
              : base(context)
        {
        }
        
        public async Task<Investigation> CreateAsync(Investigation investigation, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (investigation == null)
            {
                throw new ArgumentNullException(nameof(investigation));
            }
            Context.Investigations.Add(investigation);
            await SaveChanges(cancellationToken);
            return investigation;
        }
        public async Task<Investigation> UpdateAsync(Investigation investigation, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (investigation == null)
            {
                throw new ArgumentNullException(nameof(investigation));
            }
            Context.Investigations.Update(investigation);
            await SaveChanges(cancellationToken);
            return investigation;
        }
        public async Task<Investigation> DeleteAsync(Investigation investigation, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (investigation == null)
            {
                throw new ArgumentNullException(nameof(investigation));
            }
            Context.Investigations.Remove(investigation);
            await SaveChanges(cancellationToken);
            return investigation;
        }

        public async Task<Investigation> FindByIdAsync(Guid investigationId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (investigationId == null)
            {
                throw new ArgumentNullException(nameof(investigationId));
            }
            return await Context.Investigations.FirstOrDefaultAsync(f => f.Id == investigationId);
        }

        public async Task<Investigation> FindByResumeIdAsync(Guid resumeId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (resumeId == null)
            {
                throw new ArgumentNullException(nameof(resumeId));
            }
            return await Context.Investigations.FirstOrDefaultAsync(f => f.ResumeId == resumeId);
        }


    }
}
