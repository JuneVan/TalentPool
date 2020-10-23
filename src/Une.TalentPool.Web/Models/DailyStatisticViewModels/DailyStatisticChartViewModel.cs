using System.Collections.Generic;

namespace Une.TalentPool.Web.Models.DailyStatisticViewModels
{
    public class DailyStatisticChartViewModel
    {
        public List<string> Labels { get; set; }
        public List<ChartItemModel> UpdateData { get; set; }
        public List<ChartItemModel> DownloadData { get; set; } 
    }
    public class ChartItemModel
    {
        public string Label { get; set; }
        public List<int> Values { get; set; }
    }
}
