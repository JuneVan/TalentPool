using System;
using Van.TalentPool.Entities;

namespace Van.TalentPool.Resumes
{
    public class ResumeKeyMap:Entity
    {
        public virtual string Keyword { get; set; }
        public virtual Guid ResumeId { get; set; }
    }
}
