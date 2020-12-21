using System;
using TalentPool.Interviews;

namespace TalentPool.Application.Interviews
{
    public class UnfinshInterviewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string JobName { get; set; }
        public string PhoneNumber { get; set; }
        public Guid CreatorUserId { get; set; }
        public string CreatorUserName { get; set; }
        public Guid OwnerUserId { get; set; }
        public string OwnerUserName { get; set; }

        public InterviewStatus Status { get; set; } 
        public DateTime CreationTime { get; set; }
    }
}
