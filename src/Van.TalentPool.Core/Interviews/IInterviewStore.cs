using System;
using System.Threading;
using System.Threading.Tasks;

namespace Van.TalentPool.Interviews
{
    public interface IInterviewStore
    {
        Task<Interview> CreateAsync(Interview interview, CancellationToken cancellationToken);
        Task<Interview> UpdateAsync(Interview interview, CancellationToken cancellationToken);
        Task<Interview> DeleteAsync(Interview interview, CancellationToken cancellationToken);
        Task<Interview> FindByIdAsync(Guid interviewId, CancellationToken cancellationToken);
    }
}
