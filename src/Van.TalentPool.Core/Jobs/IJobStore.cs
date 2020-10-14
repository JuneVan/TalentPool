using System;
using System.Threading;
using System.Threading.Tasks;

namespace Van.TalentPool.Jobs
{
    public interface IJobStore : IDisposable
    {
        Task<Job> CreateAsync(Job job, CancellationToken cancellationToken);
        Task<Job> UpdateAsync(Job job, CancellationToken cancellationToken);
        Task<Job> DeleteAsync(Job job, CancellationToken cancellationToken);
        Task<Job> FindByIdAsync(Guid jobId, CancellationToken cancellationToken);
    }
}
