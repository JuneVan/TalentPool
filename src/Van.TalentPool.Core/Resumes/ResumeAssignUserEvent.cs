using MediatR;
using System;

namespace Van.TalentPool.Resumes
{
    public class ResumeAssignUserEvent : INotification
    {
        public Guid ResumeId { get; set; }
        public Guid OwnerUserId { get; set; }
    }
}
