using System.Collections.Generic;
using Une.TalentPool.Application.Interviews;
using Une.TalentPool.Application.Resumes;

namespace Une.TalentPool.Web.Models.IndexViewModels
{
    public class IndexViewModel
    { 
        // 月新增数据
        public MonthlyIncreaseData MonthlyIncreaseData { get; set; }
        // 月简历数统计   
        public MonthlyData ResumeMonthlyData { get; set; }
        // 月简历数统计   
        public MonthlyData InvestigaionMonthlyData { get; set; }

        // 今日简历数
        public TodayResumeData TodayResumeData { get; set; }
        // 今日调查数
        public TodayInvestigationData TodayInvestigationData { get; set; }

        // 简历排行榜
        public RankData ResumeRankData { get; set; }
        // 调查排行榜
        public RankData InvestigaionRankData { get; set; }

        
        //待处理的简历
        public List<UncompleteResumeDto> TodoTasks { get; set; }

        // 预约任务
        public List<UnfinshInterviewDto> InterviewTasks { get; set; }
    }

    public class MonthlyIncreaseData
    {
        // 月新增简历
        public int NewResumeCount { get; set; }
        public int NewResumeTotalCount { get; set; }
        // 月新增调查
        public int NewInvestigationCount { get; set; }
        public int NewInvestigationTotalCount { get; set; }
        // 月新增预约
        public int NewInterviewCount { get; set; }
        public int NewInterviewTotalCount { get; set; }

    }

    // 月简历数据统计
    public class MonthlyData
    {
        public List<string> Labels { get; set; }
        public List<StatisticsModel> Datasets { get; set; }

    }
    public class StatisticsModel
    {
        public string Label { get; set; }
        public List<int> Values { get; set; }
    }

    public class TodayResumeData
    {
        /// <summary>
        /// 不合格简历
        /// </summary>
        public int UnpassedCount { get; set; }
        /// <summary>
        /// 合格简历
        /// </summary>
        public int PassedCount { get; set; }

        /// <summary>
        /// 未处理简历数
        /// </summary>
        public int UnhandledCount { get; set; }
    }
    public class TodayInvestigationData
    {
        //接受人数
        public int AcceptCount { get; set; }
        //拒绝人数
        public int RefuseCount { get; set; }
        //考虑
        public int ConsiderCount { get; set; }
        //未接人数
        public int MissedCount { get; set; }
    }
    public class RankData
    {
        public List<UserRankData> MonthlyUserRanks { get; set; }
        public List<UserRankData> WeekUserRanks { get; set; }
        public List<UserRankData> DayUserRanks { get; set; }
    }

    public class UserRankData
    {
        public string Photo { get; set; }
        public string FullName { get; set; }
        public int TotalCount { get; set; }
        public int QualifiedCount { get; set; }
    }  
}
