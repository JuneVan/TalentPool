using System;
using Une.TalentPool.Interviews;

namespace Une.TalentPool.Web.Models.InterviewViewModels
{
    public class ChangeInterviewViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public InterviewStatus Status { get; set; }
        public DateTime VisitedTime { get; set; }
        public string Remark { get; set; }
    }
}
