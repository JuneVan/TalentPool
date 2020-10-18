using System;
using Une.TalentPool.Entities;

namespace Une.TalentPool.Resumes
{
    public class ResumeKeywordMap : Entity
    {
        public virtual string Name { get; set; }
        public virtual string Keyword { get; set; } 
        public virtual Guid ResumeId { get; set; }
    }
}
