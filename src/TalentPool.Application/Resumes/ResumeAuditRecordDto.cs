using System;

namespace TalentPool.Application.Resumes
{
    public class ResumeAuditRecordDto : Dto
    { 
        public DateTime CreationTime { get; set; }
        public Guid CreatorUserId { get; set; }
        public string CreatorUserName { get; set; }
        public string Remark { get; set; }
        public bool Passed { get; set; }
    }
}
