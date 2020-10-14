using System;
using System.Threading;
using System.Threading.Tasks;

namespace Van.TalentPool.Investigations
{
    public interface IInvestigationStore : IDisposable
    {
        Task<Investigation> CreateAsync(Investigation investigation, CancellationToken cancellationToken);
        Task<Investigation> UpdateAsync(Investigation investigation, CancellationToken cancellationToken);
        Task<Investigation> DeleteAsync(Investigation investigation, CancellationToken cancellationToken);
        Task<Investigation> FindByIdAsync(Guid investigationId, CancellationToken cancellationToken);
        Task<Investigation> FindByResumeIdAsync(Guid resumeId, CancellationToken cancellationToken);
    }
}
