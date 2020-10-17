using System;

namespace Une.TalentPool.Application.Jobs
{
    public class JobDto : Dto
    {
        public string Title { get; set; }
        public bool Enable { get; set; }
        public string SalaryRange { get; set; }
        public string GenderRange { get; set; }
        public string AgeRange { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
