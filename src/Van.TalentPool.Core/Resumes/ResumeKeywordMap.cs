using System;
using Van.TalentPool.Entities;

namespace Van.TalentPool.Resumes
{
    public class ResumeKeywordMap : Entity
    {
        public virtual string Name { get; set; }
        public virtual string Keyword { get; set; }
        public virtual string OriginData { get; set; }
        public virtual Guid ResumeId { get; set; }
    }
}
