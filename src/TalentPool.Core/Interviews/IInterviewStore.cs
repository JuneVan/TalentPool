using System;
using System.Threading;
using System.Threading.Tasks;

namespace TalentPool.Interviews
{
    public interface IInterviewStore : IDisposable
    {
        Task<Interview> CreateAsync(Interview interview, CancellationToken cancellationToken);
        Task<Interview> UpdateAsync(Interview interview, CancellationToken cancellationToken);
        Task<Interview> DeleteAsync(Interview interview, CancellationToken cancellationToken);
        Task<Interview> FindByIdAsync(Guid interviewId, CancellationToken cancellationToken);
    }
}
