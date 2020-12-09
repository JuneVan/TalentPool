using System;
using TalentPool.Interviews;

namespace TalentPool.Application.Interviews
{
    public class ReportInterviewDto
    { 
        public string Name { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string JobName { get; set; }
        public DateTime? VisitedTime { get; set; }
        public string Remark { get; set; }
        public InterviewStatus Status { get; set; }
    }
}
