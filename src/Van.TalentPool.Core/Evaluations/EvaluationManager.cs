using System;
using System.Threading;
using System.Threading.Tasks;

namespace Van.TalentPool.Evaluations
{
    public class EvaluationManager : IDisposable
    {
        private bool _disposed;
        public EvaluationManager(IEvaluationStore evaluationStore)
        {
            EvaluationStore = evaluationStore;
        }
        protected IEvaluationStore EvaluationStore { get; }
        protected virtual CancellationToken CancellationToken => CancellationToken.None;
        public async Task<Evaluation> CreateAsync(Evaluation evaluation)
        {
            if (evaluation == null)
                throw new ArgumentNullException(nameof(evaluation));

            return await EvaluationStore.CreateAsync(evaluation, CancellationToken);
        }

        public async Task<Evaluation> UpdateAsync(Evaluation evaluation)
        {
            if (evaluation == null)
                throw new ArgumentNullException(nameof(evaluation));

            return await EvaluationStore.UpdateAsync(evaluation, CancellationToken);
        }
        public async Task<Evaluation> DeleteAsync(Evaluation evaluation)
        {
            if (evaluation == null)
                throw new ArgumentNullException(nameof(evaluation));

            return await EvaluationStore.DeleteAsync(evaluation, CancellationToken);
        }
        public async Task<Evaluation> FindByIdAsync(Guid evaluationId)
        {
            if (evaluationId == null)
                throw new ArgumentNullException(nameof(evaluationId));

            return await EvaluationStore.FindByIdAsync(evaluationId, CancellationToken);
        } 
        #region Subject
        public async Task<EvaluationSubject> CreateSubjectAsync(EvaluationSubject subject)
        {
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));

            return await EvaluationStore.CreateSubjectAsync(subject, CancellationToken);
        }

        public async Task<EvaluationSubject> UpdateSubjectAsync(EvaluationSubject subject)
        {
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));

            return await EvaluationStore.UpdateSubjectAsync(subject, CancellationToken);
        }
        public async Task<EvaluationSubject> DeleteSubjectAsync(EvaluationSubject subject)
        {
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));

            return await EvaluationStore.DeleteSubjectAsync(subject, CancellationToken);
        }
        public async Task<EvaluationSubject> FindSubjectByIdAsync(Guid evaluationId)
        {
            if (evaluationId == null)
                throw new ArgumentNullException(nameof(evaluationId));

            return await EvaluationStore.FindSubjectByIdAsync(evaluationId, CancellationToken);
        }
        #endregion

        #region Question
        public async Task<EvaluationQuestion> CreateQuestionAsync(EvaluationQuestion question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            return await EvaluationStore.CreateQuestionAsync(question, CancellationToken);
        }

        public async Task<EvaluationQuestion> UpdateQuestionAsync(EvaluationQuestion question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            return await EvaluationStore.UpdateQuestionAsync(question, CancellationToken);
        }
        public async Task<EvaluationQuestion> DeleteQuestionAsync(EvaluationQuestion question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            return await EvaluationStore.DeleteQuestionAsync(question, CancellationToken);
        }
        public async Task<EvaluationQuestion> FindQuestionByIdAsync(Guid evaluationId)
        {
            if (evaluationId == null)
                throw new ArgumentNullException(nameof(evaluationId));

            return await EvaluationStore.FindQuestionByIdAsync(evaluationId, CancellationToken);
        }
        #endregion


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                EvaluationStore.Dispose();
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
