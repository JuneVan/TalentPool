using System;

namespace Van.TalentPool.Application.Interviews
{
    public class QueryInterviewInput : PaginationInput
    {
        public string Keyword { get; set; }
        public Guid? CreatorUserId { get; set; }
        public DateTime? StartTime { get; set; } = DateTime.Now.Date;
        public DateTime? EndTime { get; set; } = DateTime.Now.Date.AddDays(7);
        public sbyte? Status { get; set; }
    }
}
