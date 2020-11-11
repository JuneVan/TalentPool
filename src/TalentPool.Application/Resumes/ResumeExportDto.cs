using System;
using TalentPool.Resumes;

namespace TalentPool.Application.Resumes
{
    public class ResumeExportDto : Dto
    {
        public string PlatformName { get; set; } 
        public string PlatformId { get; set; } 
        public Guid JobId { get; set; }
        public string JobName { get; set; }
        public string Description { get; set; }
        public AuditStatus AuditStatus { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; } 
        public DateTime CreationTime { get; set; }
        public Guid CreatorUserId { get; set; } 
        public Guid OwnerUserId { get; set; } 
    }
}
