using MediatR;
using System;

namespace Une.TalentPool.Resumes
{
    public class ResumeDeletedEvent : INotification
    {
        public Guid ResumeId { get; set; }
    }
}
