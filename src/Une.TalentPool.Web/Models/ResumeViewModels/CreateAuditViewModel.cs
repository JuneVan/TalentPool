using System;
using System.Collections.Generic;

namespace Une.TalentPool.Web.Models.ResumeViewModels
{
    public class CreateAuditViewModel
    {
        public List<AuditRecordModel> AuditRecords { get; set; }
        public bool IsEnabled { get; set; }
        public Guid ResumeId { get; set; }
        public string Remark { get; set; }
        public bool Passed { get; set; }
    }
    public class AuditRecordModel
    {
        public Guid Id { get; set; }
        public DateTime? CreationTime { get; set; }
        public Guid? CreatorUserId { get; set; } 
        public string CreatorUserName { get; set; }
        public string Remark { get; set; }
        public bool? Passed { get; set; }
    }
}
