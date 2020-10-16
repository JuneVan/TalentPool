using System;
using System.Threading;
using System.Threading.Tasks;

namespace Van.TalentPool.Evaluations
{
    public interface IEvaluationStore : IDisposable
    {
        Task<Evaluation> CreateAsync(Evaluation evaluation, CancellationToken cancellationToken);
        Task<Evaluation> UpdateAsync(Evaluation evaluation, CancellationToken cancellationToken);
        Task<Evaluation> DeleteAsync(Evaluation evaluation, CancellationToken cancellationToken);
        Task<Evaluation> FindByIdAsync(Guid evaluationId, CancellationToken cancellationToken);

        Task<EvaluationQuestion> CreateQuestionAsync(EvaluationQuestion question, CancellationToken cancellationToken);
        Task<EvaluationQuestion> UpdateQuestionAsync(EvaluationQuestion question, CancellationToken cancellationToken);
        Task<EvaluationQuestion> DeleteQuestionAsync(EvaluationQuestion question, CancellationToken cancellationToken);
        Task<EvaluationQuestion> FindQuestionByIdAsync(Guid questionId, CancellationToken cancellationToken);

        Task<EvaluationSubject> CreateSubjectAsync(EvaluationSubject subject, CancellationToken cancellationToken);
        Task<EvaluationSubject> UpdateSubjectAsync(EvaluationSubject subject, CancellationToken cancellationToken);
        Task<EvaluationSubject> DeleteSubjectAsync(EvaluationSubject subject, CancellationToken cancellationToken);
        Task<EvaluationSubject> FindSubjectByIdAsync(Guid subjectId, CancellationToken cancellationToken);
    }
}
