using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Une.TalentPool.Evaluations;

namespace Une.TalentPool.EntityFrameworkCore.Stores
{
    public class EvaluationStore : StoreBase, IEvaluationStore
    {
        public EvaluationStore(VanDbContext context) : base(context)
        {

        }
        public async Task<Evaluation> CreateAsync(Evaluation evaluation, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (evaluation == null)
            {
                throw new ArgumentNullException(nameof(evaluation));
            }
            Context.Evaluations.Add(evaluation);
            await SaveChanges(cancellationToken);
            return evaluation;
        }

        public async Task<EvaluationQuestion> CreateQuestionAsync(EvaluationQuestion question, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }
            Context.EvaluationQuestions.Add(question);
            await SaveChanges(cancellationToken);
            return question;
        }

        public async Task<EvaluationSubject> CreateSubjectAsync(EvaluationSubject subject, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }
            Context.EvaluationSubjects.Add(subject);
            await SaveChanges(cancellationToken);
            return subject;
        }

        public async Task<Evaluation> DeleteAsync(Evaluation evaluation, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (evaluation == null)
            {
                throw new ArgumentNullException(nameof(evaluation));
            }
            Context.Evaluations.Remove(evaluation);
            await SaveChanges(cancellationToken);
            return evaluation;
        }

        public async Task<EvaluationQuestion> DeleteQuestionAsync(EvaluationQuestion question, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }
            Context.EvaluationQuestions.Remove(question);
            await SaveChanges(cancellationToken);
            return question;
        }

        public async Task<EvaluationSubject> DeleteSubjectAsync(EvaluationSubject subject, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }
            Context.EvaluationSubjects.Remove(subject);
            await SaveChanges(cancellationToken);
            return subject;
        }

        public async Task<Evaluation> FindByIdAsync(Guid evaluationId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (evaluationId == null)
            {
                throw new ArgumentNullException(nameof(evaluationId));
            }
            return await Context.Evaluations.Include(i => i.Subjects).Include(i => i.Questions).FirstOrDefaultAsync(f => f.Id == evaluationId);
        }

        public async Task<EvaluationQuestion> FindQuestionByIdAsync(Guid questionId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (questionId == null)
            {
                throw new ArgumentNullException(nameof(questionId));
            }
            return await Context.EvaluationQuestions.FirstOrDefaultAsync(f => f.Id == questionId);
        }

        public async Task<EvaluationSubject> FindSubjectByIdAsync(Guid subjectId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (subjectId == null)
            {
                throw new ArgumentNullException(nameof(subjectId));
            }
            return await Context.EvaluationSubjects.FirstOrDefaultAsync(f => f.Id == subjectId);
        }

        public async Task<Evaluation> UpdateAsync(Evaluation evaluation, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (evaluation == null)
            {
                throw new ArgumentNullException(nameof(evaluation));
            }
            Context.Evaluations.Update(evaluation);
            await SaveChanges(cancellationToken);
            return evaluation;
        }

        public async Task<EvaluationQuestion> UpdateQuestionAsync(EvaluationQuestion question, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }
            Context.EvaluationQuestions.Update(question);
            await SaveChanges(cancellationToken);
            return question;
        }

        public async Task<EvaluationSubject> UpdateSubjectAsync(EvaluationSubject subject, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }
            Context.EvaluationSubjects.Update(subject);
            await SaveChanges(cancellationToken);
            return subject;
        }
    }
}
