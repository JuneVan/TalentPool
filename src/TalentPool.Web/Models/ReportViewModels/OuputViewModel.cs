using System;
using System.Collections.Generic;
using TalentPool.Application.Interviews;
using TalentPool.Application.Investigations;

namespace TalentPool.Web.Models.ReportViewModels
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
        // 职位统计
        public JobStatisticTotalModel JobStatisticTotalInfo { get; set; }

        // 今日面试情况
        public InterviewStatisticTotalModel InterviewStatisticTotalInfo { get; set; }

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
    public class JobStatisticTotalModel
    {
        //搜索
        public int SearchCount { get; set; }
        //投递
        public int DeliveryCount { get; set; }
        public List<JobStatisticModel> JobStatisticInfo { get; set; }
    }
    public class JobStatisticModel
    {
        public string JobName { get; set; }
        public int Count { get; set; }
    }

    public class InterviewStatisticTotalModel
    {
        public List<InterviewStatisModel> InterviewStatisModels { get; set; }

        public List<UninterviewModel> UninterviewModels { get; set; }
    }
    public class InterviewStatisModel
    {
        /// <summary>
        /// 职位
        /// </summary>
        public string JobName { get; set; }
        /// <summary>
        /// 应到人数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 实到人数
        /// </summary>
        public int VisitedCount { get; set; } 

    }
    /// <summary>
    /// 未到人员统计
    /// </summary>
    public class UninterviewModel
    {
        public string Name { get; set; }
        public string JobName { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string Remark { get; set; }
    }

}
