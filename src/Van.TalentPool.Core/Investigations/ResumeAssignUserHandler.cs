using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Van.TalentPool.Resumes;

namespace Van.TalentPool.Investigations
{
    public class ResumeAssignUserHandler : INotificationHandler<ResumeAssignUserEvent>
    {
        private readonly InvestigationManager _investigationManager;
        public ResumeAssignUserHandler(InvestigationManager investigationManager)
        {
            _investigationManager = investigationManager;
        }
        public async Task Handle(ResumeAssignUserEvent notification, CancellationToken cancellationToken)
        {
            var investigaion = await _investigationManager.FindByResumeIdAsync(notification.ResumeId);
            if (investigaion != null)
            {
                investigaion.OwnerUserId = notification.OwnerUserId;
                await _investigationManager.UpdateAsync(investigaion);
            }
        }
    }
}
