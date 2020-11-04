using System;
using TalentPool.Interviews;

namespace TalentPool.Application.Interviews
{
    public class InterviewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string JobName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime AppointmentTime { get; set; }
        public DateTime? VisitedTime { get; set; }
        public Guid CreatorUserId { get; set; }
        public string CreatorUserName { get; set; }
        public DateTime CreationTime { get; set; }
        public InterviewStatus Status { get; set; }
        public string Remark { get; set; }
    }
}
