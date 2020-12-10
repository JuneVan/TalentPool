using System;
using System.Threading;
using System.Threading.Tasks;

namespace TalentPool.Evaluations
{
    public class EvaluationManager : ObjectDisposable
    {  
        public EvaluationManager(IEvaluationStore evaluationStore,
            ISignal signal)
        {
            EvaluationStore = evaluationStore;
            Signal = signal;
        }
        protected ISignal  Signal { get; }
        protected IEvaluationStore EvaluationStore { get; }
        protected virtual CancellationToken CancellationToken => Signal.Token;
        public async Task<Evaluation> CreateAsync(Evaluation evaluation)
        {
            ThrowIfDisposed();
            if (evaluation == null)
                throw new ArgumentNullException(nameof(evaluation));

            return await EvaluationStore.CreateAsync(evaluation, CancellationToken);
        }

        public async Task<Evaluation> UpdateAsync(Evaluation evaluation)
        {
            ThrowIfDisposed();
            if (evaluation == null)
                throw new ArgumentNullException(nameof(evaluation));

            return await EvaluationStore.UpdateAsync(evaluation, CancellationToken);
        }
        public async Task<Evaluation> DeleteAsync(Evaluation evaluation)
        {
            ThrowIfDisposed();
            if (evaluation == null)
                throw new ArgumentNullException(nameof(evaluation));

            return await EvaluationStore.DeleteAsync(evaluation, CancellationToken);
        }
        public async Task<Evaluation> FindByIdAsync(Guid evaluationId)
        {
            ThrowIfDisposed();
            if (evaluationId == null)
                throw new ArgumentNullException(nameof(evaluationId));

            return await EvaluationStore.FindByIdAsync(evaluationId, CancellationToken);
        }
        #region Subject
        public async Task<EvaluationSubject> CreateSubjectAsync(EvaluationSubject subject)
        {
            ThrowIfDisposed();
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));

            return await EvaluationStore.CreateSubjectAsync(subject, CancellationToken);
        }

        public async Task<EvaluationSubject> UpdateSubjectAsync(EvaluationSubject subject)
        {
            ThrowIfDisposed();
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));

            return await EvaluationStore.UpdateSubjectAsync(subject, CancellationToken);
        }
        public async Task<EvaluationSubject> DeleteSubjectAsync(EvaluationSubject subject)
        {
            ThrowIfDisposed();
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));

            return await EvaluationStore.DeleteSubjectAsync(subject, CancellationToken);
        }
        public async Task<EvaluationSubject> FindSubjectByIdAsync(Guid evaluationId)
        {
            ThrowIfDisposed();
            if (evaluationId == null)
                throw new ArgumentNullException(nameof(evaluationId));

            return await EvaluationStore.FindSubjectByIdAsync(evaluationId, CancellationToken);
        }
        #endregion

        #region Question
        public async Task<EvaluationQuestion> CreateQuestionAsync(EvaluationQuestion question)
        {
            ThrowIfDisposed();
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            return await EvaluationStore.CreateQuestionAsync(question, CancellationToken);
        }

        public async Task<EvaluationQuestion> UpdateQuestionAsync(EvaluationQuestion question)
        {
            ThrowIfDisposed();
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            return await EvaluationStore.UpdateQuestionAsync(question, CancellationToken);
        }
        public async Task<EvaluationQuestion> DeleteQuestionAsync(EvaluationQuestion question)
        {
            ThrowIfDisposed();
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            return await EvaluationStore.DeleteQuestionAsync(question, CancellationToken);
        }
        public async Task<EvaluationQuestion> FindQuestionByIdAsync(Guid evaluationId)
        {
            ThrowIfDisposed();
            if (evaluationId == null)
                throw new ArgumentNullException(nameof(evaluationId));

            return await EvaluationStore.FindQuestionByIdAsync(evaluationId, CancellationToken);
        }
        #endregion

         

    }
}
