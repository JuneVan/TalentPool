using System;
using Van.TalentPool.Resumes;

namespace Van.TalentPool.Application.Resumes
{
    public class MonthlyResumeDto
    {
        public Guid CreatorUserId { get; set; }
        public string CreatorUserName { get; set; }
        public string CreatorUserPhoto { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid OwnerUserId { get; set; } 
        public AuditStatus AuditStatus { get; set; }
    }
}
