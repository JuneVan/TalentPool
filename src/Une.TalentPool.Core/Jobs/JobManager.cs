using System;
using System.Threading;
using System.Threading.Tasks;

namespace Une.TalentPool.Jobs
{
    public class JobManager : IDisposable
    {
        private bool _disposed;
        private readonly ITokenProvider _tokenProvider;
        public JobManager(IJobStore jobStore,
            ITokenProvider tokenProvider)
        {
            JobStore = jobStore;
            _tokenProvider = tokenProvider;
        }

        protected IJobStore JobStore { get; }
        protected virtual CancellationToken CancellationToken => _tokenProvider.Token;

        public async Task<Job> CreateAsync(Job job)
        {
            if (job == null)
                throw new ArgumentNullException(nameof(job));
            return await JobStore.CreateAsync(job, CancellationToken);
        }

        public async Task<Job> UpdateAsync(Job job)
        {
            if (job == null)
                throw new ArgumentNullException(nameof(job));
            return await JobStore.UpdateAsync(job, CancellationToken);
        }
        public async Task<Job> DeleteAsync(Job job)
        {
            if (job == null)
                throw new ArgumentNullException(nameof(job));
            return await JobStore.DeleteAsync(job, CancellationToken);
        }
        public async Task<Job> FindByIdAsync(Guid jobId)
        {
            if (jobId == null)
                throw new ArgumentNullException(nameof(jobId));
            return await JobStore.FindByIdAsync(jobId, CancellationToken);
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
                JobStore.Dispose();
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
