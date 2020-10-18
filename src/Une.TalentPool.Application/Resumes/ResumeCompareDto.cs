using System;

namespace Une.TalentPool.Application.Resumes
{
    public  class ResumeCompareDto
    {
        public Guid RelationResumeId { get; set; }
        public string RelationResumeName { get; set; }
        public decimal Similarity { get; set; }
    }
}
