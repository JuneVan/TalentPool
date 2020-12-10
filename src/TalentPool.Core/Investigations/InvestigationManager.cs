using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TalentPool.Investigations
{
    public class InvestigationManager : ObjectDisposable
    {
        public InvestigationManager(IInvestigationStore investigaionStore,
            IEnumerable<IInvestigaionValidator> investigaionValidators,
            ISignal signal)
        {
            InvestigaionStore = investigaionStore;
            InvestigaionValidators = investigaionValidators;
            Signal = signal;
        }

        protected ISignal Signal { get; }
        protected IInvestigationStore InvestigaionStore { get; }
        protected CancellationToken CancellationToken => Signal.Token;
        protected IEnumerable<IInvestigaionValidator> InvestigaionValidators { get; }

        public async Task<Investigation> CreateAsync(Investigation investigation)
        {
            ThrowIfDisposed();
            if (investigation == null)
                throw new ArgumentNullException(nameof(investigation));
            await ValidateAsync(investigation);
            return await InvestigaionStore.CreateAsync(investigation, CancellationToken);
        }
        public async Task<Investigation> UpdateAsync(Investigation investigation)
        {
            ThrowIfDisposed();
            if (investigation == null)
                throw new ArgumentNullException(nameof(investigation));
            return await InvestigaionStore.UpdateAsync(investigation, CancellationToken);
        }
        private async Task ValidateAsync(Investigation investigation)
        {
            ThrowIfDisposed();
            if (InvestigaionValidators != null)
            {
                foreach (var validator in InvestigaionValidators)
                {
                    await validator.ValidateAsync(this, investigation);
                }
            }
        } 
        public async Task CompleteAsync(Investigation investigation)
        {
            ThrowIfDisposed();
            if (investigation == null)
                throw new ArgumentNullException(nameof(investigation));
            investigation.Status = InvestigationStatus.Complete;
            await InvestigaionStore.UpdateAsync(investigation, CancellationToken);
        }
        public async Task RestoreAsync(Investigation investigation)
        {
            ThrowIfDisposed();
            if (investigation == null)
                throw new ArgumentNullException(nameof(investigation));
            investigation.Status = InvestigationStatus.Ongoing;
            await InvestigaionStore.UpdateAsync(investigation, CancellationToken);
        }
        public async Task AuditAsync(Investigation investigation, bool isQualified, string remark)
        {
            ThrowIfDisposed();
            if (investigation == null)
                throw new ArgumentNullException(nameof(investigation));

            if (investigation.Status == InvestigationStatus.NoStart)
                throw new InvalidOperationException("意向调查还未开始，无法进行审核。");

            investigation.IsQualified = isQualified;
            investigation.QualifiedRemark = remark;

            await InvestigaionStore.UpdateAsync(investigation, CancellationToken);
        }
        public async Task DeleteAsync(Investigation investigation)
        {
            ThrowIfDisposed();
            if (investigation == null)
                throw new ArgumentNullException(nameof(investigation));
            await InvestigaionStore.DeleteAsync(investigation, CancellationToken);
        }

        public async Task<Investigation> FindByIdAsync(Guid investigationId)
        {
            ThrowIfDisposed();
            if (investigationId == null)
                throw new ArgumentNullException(nameof(investigationId));
            return await InvestigaionStore.FindByIdAsync(investigationId, CancellationToken);
        }
        public async Task<Investigation> FindByResumeIdAsync(Guid resumeId)
        {
            ThrowIfDisposed();
            if (resumeId == null)
                throw new ArgumentNullException(nameof(resumeId));
            return await InvestigaionStore.FindByResumeIdAsync(resumeId, CancellationToken);
        }

        public async Task EvaluateAsync(Investigation investigation, string evaluation)
        {
            ThrowIfDisposed();
            if (investigation == null)
                throw new ArgumentNullException(nameof(investigation));
            if (evaluation == null)
                throw new ArgumentNullException(nameof(evaluation));
            investigation.Evaluation = evaluation;
            await UpdateAsync(investigation);
        } 
    }
}
