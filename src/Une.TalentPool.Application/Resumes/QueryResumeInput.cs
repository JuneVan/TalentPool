using System;

namespace Une.TalentPool.Application.Resumes
{
    public class QueryResumeInput : PaginationInput
    {
        public string Keyword { get; set; }
        public Guid? JobId { get; set; }
        public Guid? CreatorUserId { get; set; }
        public Guid? OwnerUserId { get; set; }
        public DateTime? StartTime { get; set; } = DateTime.Now.Date;
        public DateTime? EndTime { get; set; } = DateTime.Now.Date.AddDays(1);
        public sbyte? AuditStatus { get; set; }
    }
}
