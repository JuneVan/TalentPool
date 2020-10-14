using System;

namespace Van.TalentPool.Application.Investigations
{
    public class QueryInvestigaionInput : PaginationInput
    {
        public string Keyword { get; set; }
        public Guid? JobId { get; set; }
        public Guid? OwnerUserId { get; set; }
        public sbyte? Status { get; set; }
        public DateTime? StartTime { get; set; } = DateTime.Now.Date;
        public DateTime? EndTime { get; set; } = DateTime.Now.Date.AddDays(1);
    }
}
