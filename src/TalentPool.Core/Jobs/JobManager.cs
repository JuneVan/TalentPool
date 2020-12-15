using System;
using System.Threading;
using System.Threading.Tasks;

namespace TalentPool.Jobs
{
    public class JobManager : ObjectDisposable
    { 
        public JobManager(IJobStore jobStore,
            ISignal signal)
        {
            JobStore = jobStore;
            Signal = signal;
        }

        protected ISignal Signal { get; }
        protected IJobStore JobStore { get; }
        protected virtual CancellationToken CancellationToken => Signal.Token;

        public async Task<Job> CreateAsync(Job job)
        {
            ThrowIfDisposed();
            if (job == null)
                throw new ArgumentNullException(nameof(job));
            return await JobStore.CreateAsync(job, CancellationToken);
        }

        public async Task<Job> UpdateAsync(Job job)
        {
            ThrowIfDisposed();
            if (job == null)
                throw new ArgumentNullException(nameof(job));
            return await JobStore.UpdateAsync(job, CancellationToken);
        }
        public async Task<Job> DeleteAsync(Job job)
        {
            ThrowIfDisposed();
            if (job == null)
                throw new ArgumentNullException(nameof(job));
            return await JobStore.DeleteAsync(job, CancellationToken);
        }
        public async Task<Job> FindByIdAsync(Guid jobId)
        {
            ThrowIfDisposed();
            if (jobId == null)
                throw new ArgumentNullException(nameof(jobId));
            return await JobStore.FindByIdAsync(jobId, CancellationToken);
        }

        protected override void DisposeUnmanagedResource()
        {
            JobStore.Dispose();
        }
    }
}
