using System;
using TalentPool.Interviews;

namespace TalentPool.Application.Interviews
{
    public  class InterviewCalendarDto:Dto
    {
        public string Name { get; set; }
        public DateTime AppointmentTime { get; set; }
        public InterviewStatus Status { get; set; }
        public string JobName { get; set; }
    }
}
