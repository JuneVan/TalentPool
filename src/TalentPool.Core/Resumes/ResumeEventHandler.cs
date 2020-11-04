using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace TalentPool.Resumes
{
    public class ResumeEventHandler : INotificationHandler<ResumeDeletedEvent>
    {
        private readonly ResumeManager _resumeManager;
        public ResumeEventHandler(ResumeManager resumeManager)
        {
            _resumeManager = resumeManager;
        }
        public async Task Handle(ResumeDeletedEvent notification, CancellationToken cancellationToken)
        {
            var resumeKeywordMaps = await _resumeManager.GetResumeKeyMapsAsync(notification.ResumeId);
            if (resumeKeywordMaps != null)
            {
                await _resumeManager.RemoveResumeKeyMapsAsync(resumeKeywordMaps);
            }
        }
    }
}
