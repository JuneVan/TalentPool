using System.Collections.Generic;
using Une.TalentPool.Application.Investigations;

namespace Une.TalentPool.Web.Models.ReportViewModels
{
    public class OuputViewModel
    {
        //文本概要
        public string Summary { get; set; }

        public string Date { get; set; }

        //简历统计
        public List<ResumeStatisticModel> ResumeStatisticInfo { get; set; }
        //调查情况统计
        public List<InvestigationStatisticModel> InvestigationStatisticInfo { get; set; }
        //今日调查列表 
        public List<ReportInvestigationDto> Investigations { get; set; }

    }
    //简历城市分布统计
    public class ResumeStatisticModel
    {
        public string CreatorUserName { get; set; }
        public int Count { get; set; }
    }
    //调查情况统计
    public class InvestigationStatisticModel
    {
        public string Name { get; set; }
        //调查人数
        public int TotalCount { get; set; }
        //接受人数
        public int AcceptCount { get; set; }
        //拒绝人数
        public int RefuseCount { get; set; }
        //考虑
        public int ConsiderCount { get; set; }
        //未接人数
        public int MissedCount { get; set; }
    }
    
}
