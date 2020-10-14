using System;
using System.Threading;
using System.Threading.Tasks;

namespace Van.TalentPool.Evaluations
{
    public interface IEvaluationStore
    {
        Task<Evaluation> CreateAsync(Evaluation evaluation, CancellationToken cancellationToken);
        Task<Evaluation> UpdateAsync(Evaluation evaluation, CancellationToken cancellationToken);
        Task<Evaluation> DeleteAsync(Evaluation evaluation, CancellationToken cancellationToken);
        Task<Evaluation> FindByIdAsync(Guid evaluationId, CancellationToken cancellationToken);
    }
}
