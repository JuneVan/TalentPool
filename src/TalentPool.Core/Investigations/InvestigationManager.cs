using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TalentPool.Investigations
{
    public class InvestigationManager:IDisposable
    {
        private bool _disposed;
        private readonly ITokenProvider _tokenProvider;
        public InvestigationManager(IInvestigationStore investigaionStore,
            IEnumerable<IInvestigaionValidator> investigaionValidators,
            ITokenProvider  tokenProvider)
        {
            InvestigaionStore = investigaionStore;
            InvestigaionValidators = investigaionValidators;
            _tokenProvider = tokenProvider;
        }

        protected IInvestigationStore InvestigaionStore { get; }
        protected CancellationToken CancellationToken => _tokenProvider.Token;
        protected IEnumerable<IInvestigaionValidator> InvestigaionValidators { get; }

        public async Task<Investigation> CreateAsync(Investigation investigation)
        {
            if (investigation == null)
                throw new ArgumentNullException(nameof(investigation));
            await ValidateAsync(investigation);             
            return await InvestigaionStore.CreateAsync(investigation, CancellationToken);
        }
        public async Task<Investigation> UpdateAsync(Investigation investigation)
        {
            if (investigation == null)
                throw new ArgumentNullException(nameof(investigation));
            return await InvestigaionStore.UpdateAsync(investigation, CancellationToken);
        }
        private async Task ValidateAsync(Investigation investigation)
        {
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
            if (investigation == null)
                throw new ArgumentNullException(nameof(investigation));
            investigation.Status = InvestigationStatus.Complete;
            await InvestigaionStore.UpdateAsync(investigation, CancellationToken);
        }
        public async Task RestoreAsync(Investigation investigation)
        {
            if (investigation == null)
                throw new ArgumentNullException(nameof(investigation));
            investigation.Status = InvestigationStatus.Ongoing;
            await InvestigaionStore.UpdateAsync(investigation, CancellationToken);
        }
        public async Task AuditAsync(Investigation investigation, bool isQualified, string remark)
        {
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
            if (investigation == null)
                throw new ArgumentNullException(nameof(investigation));
            await InvestigaionStore.DeleteAsync(investigation, CancellationToken);
        }

        public async Task<Investigation> FindByIdAsync(Guid investigationId)
        {
            if (investigationId == null)
                throw new ArgumentNullException(nameof(investigationId));
            return await InvestigaionStore.FindByIdAsync(investigationId, CancellationToken);
        }
        public async Task<Investigation> FindByResumeIdAsync(Guid resumeId)
        {
            if (resumeId == null)
                throw new ArgumentNullException(nameof(resumeId));
            return await InvestigaionStore.FindByResumeIdAsync(resumeId, CancellationToken);
        }

        public async Task EvaluateAsync(Investigation investigation,string evaluation)
        {
            if (investigation == null)
                throw new ArgumentNullException(nameof(investigation));
            if (evaluation == null)
                throw new ArgumentNullException(nameof(evaluation));
            investigation.Evaluation = evaluation;
            await UpdateAsync(investigation);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                InvestigaionStore.Dispose();
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
