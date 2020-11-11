using System;

namespace TalentPool.Application.DailyStatistics
{
    public class DailyStatisticChartDto
    {
        public DateTime Date { get; set; }
        public string Platform { get; set; }
        public string JobName { get; set; }
        public int UpdateCount { get; set; }
        public int DownloadCount { get; set; }
    }
}
