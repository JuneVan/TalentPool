using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TalentPool.Jobs;

namespace TalentPool.EntityFrameworkCore.Stores
{
    public class JobStore : StoreBase, IJobStore
    {
        public JobStore(TalentDbContext context) : base(context)
        {

        }

        public async Task<Job> CreateAsync(Job job, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (job == null)
                throw new ArgumentNullException(nameof(job));
            Context.Jobs.Add(job);
            await SaveChanges(cancellationToken);
            return job;
        }

        public async Task<Job> UpdateAsync(Job job, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (job == null)
                throw new ArgumentNullException(nameof(job));
            Context.Jobs.Attach(job);
            job.ConcurrencyStamp = Guid.NewGuid().ToString();
            Context.Jobs.Update(job);
            try
            {
                await SaveChanges(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException("Optimistic concurrency failure, object has been modified.");
            }
            return job;
        }
        public async Task<Job> DeleteAsync(Job job, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (job == null)
                throw new ArgumentNullException(nameof(job));
            Context.Jobs.Remove(job);
            await SaveChanges(cancellationToken);
            return job;
        }

        public async Task<Job> FindByIdAsync(Guid jobId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (jobId == null)
                throw new ArgumentNullException(nameof(jobId));
            return await Context.Jobs.FirstOrDefaultAsync(f => f.Id == jobId, cancellationToken);

        }

    }
}
