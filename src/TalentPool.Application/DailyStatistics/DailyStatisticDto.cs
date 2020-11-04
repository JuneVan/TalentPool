using System;

namespace TalentPool.Application.DailyStatistics
{
    public class DailyStatisticDto : Dto
    { 
        public string Platform { get; set; } 
        public DateTime Date { get; set; }
        public DateTime CreationTime { get; set; }
        public string CreatorUserName { get; set; }
        public string Description { get; set; }
    }
}
