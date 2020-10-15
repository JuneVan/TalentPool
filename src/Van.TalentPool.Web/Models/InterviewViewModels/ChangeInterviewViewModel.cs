using System;
using Van.TalentPool.Interviews;

namespace Van.TalentPool.Web.Models.InterviewViewModels
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
