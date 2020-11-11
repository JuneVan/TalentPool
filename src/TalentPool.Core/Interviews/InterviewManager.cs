using System;
using System.Threading;
using System.Threading.Tasks;

namespace TalentPool.Interviews
{
    public class InterviewManager:IDisposable
    {
        private bool _disposed;
        private readonly ITokenProvider _tokenProvider;
        public InterviewManager(
            IInterviewStore interviewStore,
            ITokenProvider  tokenProvider)
        {
            InterviewStore = interviewStore;
            _tokenProvider = tokenProvider;
        }
        protected   IInterviewStore InterviewStore;
        protected virtual CancellationToken CancellationToken => _tokenProvider.Token;
        public async Task<Interview> CreateAsync(Interview interview)
        {
            if (interview == null)
                throw new ArgumentNullException(nameof(interview));
            return await InterviewStore.CreateAsync(interview, CancellationToken);
        }

        public async Task<Interview> UpdateAsync(Interview interview)
        {
            if (interview == null)
                throw new ArgumentNullException(nameof(interview));
            return await InterviewStore.UpdateAsync(interview, CancellationToken);
        }

        public async Task DeleteAsync(Interview interview)
        {
            if (interview == null)
                throw new ArgumentNullException(nameof(interview));
            await InterviewStore.DeleteAsync(interview, CancellationToken);
        }
        public async Task CancelAsync(Interview interview)
        {
            if (interview == null)
                throw new ArgumentNullException(nameof(interview));

            interview.Status = InterviewStatus.Cancel;
            interview.Remark = "已取消预约";
            await InterviewStore.UpdateAsync(interview, CancellationToken);
        }
        public async Task<Interview> FindByIdAsync(Guid interviewId)
        {
            if (interviewId == null)
                throw new ArgumentNullException(nameof(interviewId));
            return await InterviewStore.FindByIdAsync(interviewId, CancellationToken);
        }
        public async Task ChangeAsync(Interview interview)
        {
            if (interview == null)
                throw new ArgumentNullException(nameof(interview));

            if (interview.Status != InterviewStatus.Arrived)
                interview.VisitedTime = DateTime.MinValue;

            await InterviewStore.UpdateAsync(interview, CancellationToken);
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
                InterviewStore.Dispose();
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
