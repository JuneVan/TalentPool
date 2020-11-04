using MediatR;
using System;

namespace TalentPool.Resumes
{
    public class ResumeDeletedEvent : INotification
    {
        public Guid ResumeId { get; set; }
    }
}
