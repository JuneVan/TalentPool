using System;
using System.Threading;
using System.Threading.Tasks;

namespace TalentPool.Interviews
{
    public class InterviewManager : ObjectDisposable
    {

        public InterviewManager(
             ISignal signal,
            IInterviewStore interviewStore
           )
        {
            Signal = signal;
            InterviewStore = interviewStore;
        }
        protected ISignal Signal { get; }
        protected IInterviewStore InterviewStore { get; }
        protected virtual CancellationToken CancellationToken => Signal.Token;
        public async Task<Interview> CreateAsync(Interview interview)
        {
            ThrowIfDisposed();
            if (interview == null)
                throw new ArgumentNullException(nameof(interview));
            return await InterviewStore.CreateAsync(interview, CancellationToken);
        }

        public async Task<Interview> UpdateAsync(Interview interview)
        {
            ThrowIfDisposed();
            if (interview == null)
                throw new ArgumentNullException(nameof(interview));
            return await InterviewStore.UpdateAsync(interview, CancellationToken);
        }

        public async Task DeleteAsync(Interview interview)
        {
            ThrowIfDisposed();
            if (interview == null)
                throw new ArgumentNullException(nameof(interview));
            await InterviewStore.DeleteAsync(interview, CancellationToken);
        }
        public async Task CancelAsync(Interview interview)
        {
            ThrowIfDisposed();
            if (interview == null)
                throw new ArgumentNullException(nameof(interview));

            interview.Status = InterviewStatus.Cancel;
            interview.Remark = "已取消预约";
            await InterviewStore.UpdateAsync(interview, CancellationToken);
        }
        public async Task<Interview> FindByIdAsync(Guid interviewId)
        {
            ThrowIfDisposed();
            if (interviewId == null)
                throw new ArgumentNullException(nameof(interviewId));
            return await InterviewStore.FindByIdAsync(interviewId, CancellationToken);
        }
        public async Task ChangeAsync(Interview interview)
        {
            ThrowIfDisposed();
            if (interview == null)
                throw new ArgumentNullException(nameof(interview));

            if (interview.Status != InterviewStatus.Arrived)
                interview.VisitedTime = DateTime.MinValue;

            await InterviewStore.UpdateAsync(interview, CancellationToken);
        }
        protected override void DisposeUnmanagedResource()
        {
            InterviewStore.Dispose();
        }
    }
}
