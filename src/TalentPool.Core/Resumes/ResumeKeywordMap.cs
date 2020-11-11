using System;
using TalentPool.Entities;

namespace TalentPool.Resumes
{
    public class ResumeKeywordMap : Entity, IHasDeletionTime
    {
        public virtual string Name { get; set; }
        public virtual string Keyword { get; set; }
        public virtual Guid ResumeId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
