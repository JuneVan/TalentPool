using System;
using TalentPool.Interviews;

namespace TalentPool.Application.Interviews
{
    public class InterviewDetailDto
    {
        public string Name { get; set; }
        public string JobName { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string Remark { get; set; }
        public string CreatorUserName { get; set; }
        public InterviewStatus Status { get; set; }
        public DateTime? VisitedTime { get; set; }
        public DateTime CreationTime { get; set; }
        public string LastModifierUserName { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
