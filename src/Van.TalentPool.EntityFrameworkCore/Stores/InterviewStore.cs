using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Van.TalentPool.Interviews;

namespace Van.TalentPool.EntityFrameworkCore.Stores
{
    public class InterviewStore : StoreBase, IInterviewStore
    {
        public InterviewStore(VanDbContext context) : base(context)
        {

        }

        public async Task<Interview> CreateAsync(Interview interview, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (interview == null)
                throw new ArgumentNullException(nameof(interview));
            Context.Interviews.Add(interview);
            await SaveChanges(cancellationToken);
            return interview;
        }

        public async Task<Interview> UpdateAsync(Interview interview, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (interview == null)
                throw new ArgumentNullException(nameof(interview));
            Context.Interviews.Attach(interview);
            interview.ConcurrencyStamp = Guid.NewGuid().ToString();
            Context.Interviews.Update(interview);
            try
            {
                await SaveChanges(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException("Optimistic concurrency failure, object has been modified.");
            }
            return interview;
        }
        public async Task<Interview> DeleteAsync(Interview interview, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (interview == null)
                throw new ArgumentNullException(nameof(interview));
            Context.Interviews.Remove(interview);
            await SaveChanges(cancellationToken);
            return interview;
        }

        public async Task<Interview> FindByIdAsync(Guid interviewId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (interviewId == null)
                throw new ArgumentNullException(nameof(interviewId));
            return await Context.Interviews.FirstOrDefaultAsync(f => f.Id == interviewId, cancellationToken);

        }
    }
}
