using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Une.TalentPool.Application.Interviews;
using Une.TalentPool.Application.Investigations;
using Une.TalentPool.Application.Resumes;
using Une.TalentPool.Investigations;
using Une.TalentPool.Resumes;
using Une.TalentPool.Web.Models.IndexViewModels;

namespace Une.TalentPool.Web.Controllers
{
    public class HomeController : WebControllerBase
    {
        private readonly IResumeQuerier _resumeQuerier;
        private readonly IInvestigationQuerier _investigationQuerier;
        private readonly IInterviewQuerier _interviewQuerier;
        public HomeController(IResumeQuerier resumeQuerier,
            IInvestigationQuerier investigationQuerier,
            IInterviewQuerier interviewQuerier,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _resumeQuerier = resumeQuerier;
            _investigationQuerier = investigationQuerier;
            _interviewQuerier = interviewQuerier;
        }

        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> Index(bool onlyMyself = true)
        {

            var model = new IndexViewModel() { OnlyMyself = onlyMyself };
            //当前用户 
            var userId = UserIdentifier.UserId;
            //用于过滤导入数据的时间界限
            var setupTime = DateTime.Parse("2020-05-21 00:00:00");
            #region 参数设置
            // 今日起始时间
            var startTime = DateTime.Now.Date;
            // 今日结束时间
            var endTime = startTime.AddDays(1);
            // 本月起始时间
            var thisMonthStartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM"));
            // 本月结束时间
            var thisMonthEndTime = thisMonthStartTime.AddMonths(1);

            int dayOfWeek = (int)DateTime.Now.DayOfWeek;
            // 本周起始时间
            var thisWeekStartTime = DateTime.Now.Date.AddDays(1 - dayOfWeek);
            var thisWeekEndTime = DateTime.Now.Date.AddDays(8 - dayOfWeek);
            #endregion


            #region 月数据
            var monthlyIncreaseData = new MonthlyIncreaseData();
            var todayResumeData = new TodayResumeData();
            var todayInvestigationData = new TodayInvestigationData();

            // 简历
            var mothlyResumes = await _resumeQuerier.GetStatisticResumesAsync(thisMonthStartTime, thisMonthEndTime, null);
            // 
            monthlyIncreaseData.NewResumeCount = mothlyResumes.Count(w => w.CreatorUserId == userId);
            monthlyIncreaseData.NewResumeTotalCount = mothlyResumes.Count;

            // 简历审核情况统计
            if (onlyMyself)
            {
                var todayResumes = mothlyResumes.Where(w => w.CreatorUserId == userId && w.CreationTime >= startTime && w.CreationTime <= endTime).ToList();
                todayResumeData.PassedCount = todayResumes.Count(w => w.AuditStatus == AuditStatus.Complete && w.Enable);
                todayResumeData.UnpassedCount = todayResumes.Count - todayResumeData.PassedCount;
                todayResumeData.UnhandledCount = todayResumes.Count(w => w.AuditStatus == AuditStatus.NoStart || w.AuditStatus == AuditStatus.Ongoing);
            }
            else
            {
                var todayResumes = mothlyResumes.Where(w =>w.CreationTime >= startTime && w.CreationTime <= endTime).ToList();
                todayResumeData.PassedCount = todayResumes.Count(w => w.AuditStatus == AuditStatus.Complete && w.Enable);
                todayResumeData.UnpassedCount = todayResumes.Count - todayResumeData.PassedCount;
                todayResumeData.UnhandledCount = todayResumes.Count(w => w.AuditStatus == AuditStatus.NoStart || w.AuditStatus == AuditStatus.Ongoing);
            }
           



            // 调查
            var monthlyInvestigations = await _investigationQuerier.GetStatisticInvestigationsAsync(thisMonthStartTime, thisMonthEndTime);

            monthlyIncreaseData.NewInvestigationCount = monthlyInvestigations.Count(w => w.OwnerUserId == userId);
            monthlyIncreaseData.NewInvestigationTotalCount = monthlyInvestigations.Count;

            // 调查情况统计
            if (onlyMyself)
            {
                var todayInvestigations = monthlyInvestigations.Where(w => w.OwnerUserId == userId && w.CreationTime >= startTime && w.CreationTime <= endTime).ToList();
                todayInvestigationData.AcceptCount = todayInvestigations.Count(w => w.AcceptTravelStatus == AcceptTravelStatus.Accept);
                todayInvestigationData.RefuseCount = todayInvestigations.Count(w => w.AcceptTravelStatus == AcceptTravelStatus.Refuse);
                todayInvestigationData.ConsiderCount = todayInvestigations.Count(w => w.AcceptTravelStatus == AcceptTravelStatus.Consider);
                todayInvestigationData.MissedCount = todayInvestigations.Count(w => w.IsConnected == false);
            }
            else
            {
                var todayInvestigations = monthlyInvestigations.Where(w =>  w.CreationTime >= startTime && w.CreationTime <= endTime).ToList();
                todayInvestigationData.AcceptCount = todayInvestigations.Count(w => w.AcceptTravelStatus == AcceptTravelStatus.Accept);
                todayInvestigationData.RefuseCount = todayInvestigations.Count(w => w.AcceptTravelStatus == AcceptTravelStatus.Refuse);
                todayInvestigationData.ConsiderCount = todayInvestigations.Count(w => w.AcceptTravelStatus == AcceptTravelStatus.Consider);
                todayInvestigationData.MissedCount = todayInvestigations.Count(w => w.IsConnected == false);
            }
           

            model.MonthlyIncreaseData = monthlyIncreaseData;
            model.TodayResumeData = todayResumeData;
            model.TodayInvestigationData = todayInvestigationData;

            // 预约
            var monthlyInterviews = await _interviewQuerier.GetStatisticInterviewsAsync(startTime, endTime);

            monthlyIncreaseData.NewInterviewCount = monthlyInterviews.Count(w => w.CreatorUserId == userId);
            monthlyIncreaseData.NewInterviewTotalCount = monthlyInterviews.Count;

            // 月简历统计图
            var resumeMonthlyData = new MonthlyData()
            {
                Labels = new List<string>(),
                Datasets = new List<StatisticsModel>()
            };
            var dayCount = (thisMonthEndTime - thisMonthStartTime).TotalDays;
            for (int i = 0; i < dayCount; i++)
            {
                resumeMonthlyData.Labels.Add(thisMonthStartTime.AddDays(i).ToString("yyyy-MM-dd"));
            }
            var createdNameResumeGroups = mothlyResumes.GroupBy(g => g.CreatorUserName);

            foreach (var group in createdNameResumeGroups)
            {
                var resumesByGroup = mothlyResumes.Where(w => w.CreatorUserName == group.Key).ToList();
                var values = new List<int>();
                for (int i = 0; i < dayCount; i++)
                {
                    DateTime s = thisMonthStartTime.AddDays(i), e = thisMonthStartTime.AddDays(i + 1);
                    var count = resumesByGroup.Count(w => w.CreationTime >= s && w.CreationTime < e);
                    values.Add(count);
                }
                resumeMonthlyData.Datasets.Add(new StatisticsModel()
                {
                    Label = group.Key,
                    Values = values
                });

            }
            model.ResumeMonthlyData = resumeMonthlyData;
            // 月调查统计图
            var investigationMonthlyData = new MonthlyData()
            {
                Labels = new List<string>(),
                Datasets = new List<StatisticsModel>()
            };

            for (int i = 0; i < dayCount; i++)
            {
                investigationMonthlyData.Labels.Add(thisMonthStartTime.AddDays(i).ToString("yyyy-MM-dd"));
            }
            var handledNameInvestigationGroups = monthlyInvestigations.GroupBy(g => g.OwnerUserName);

            foreach (var group in handledNameInvestigationGroups)
            {
                var investigationByGroup = monthlyInvestigations.Where(w => w.OwnerUserName == group.Key).ToList();
                var values = new List<int>();
                for (int i = 0; i < dayCount; i++)
                {
                    DateTime s = thisMonthStartTime.AddDays(i), e = thisMonthStartTime.AddDays(i + 1);
                    var count = investigationByGroup.Count(w => w.CreationTime >= s && w.CreationTime < e);
                    values.Add(count);
                }
                investigationMonthlyData.Datasets.Add(new StatisticsModel()
                {
                    Label = group.Key,
                    Values = values
                });

            }
            model.InvestigaionMonthlyData = investigationMonthlyData;


            // 简历排行榜
            var resumeRankData = new RankData()
            {
                MonthlyUserRanks = new List<UserRankData>(),
                WeekUserRanks = new List<UserRankData>(),
                DayUserRanks = new List<UserRankData>()
            };
            foreach (var group in createdNameResumeGroups)
            {
                string photo = string.Empty;
                if (group.First() != null)
                {
                    photo = group.First()?.CreatorUserPhoto;
                }
                // 月
                var mothlyResumesByGroup = mothlyResumes.Where(w => w.CreatorUserName == group.Key).ToList();
                resumeRankData.MonthlyUserRanks.Add(new UserRankData()
                {
                    FullName = group.Key,
                    TotalCount = mothlyResumesByGroup.Count,
                    QualifiedCount = mothlyResumesByGroup.Count(c => c.AuditStatus == AuditStatus.Complete && c.Enable),
                    Photo = photo
                });


                // 周
                var weekResumesByGroup = mothlyResumes.Where(w => w.CreatorUserName == group.Key && w.CreationTime >= thisWeekStartTime && w.CreationTime < thisWeekEndTime).ToList();
                resumeRankData.WeekUserRanks.Add(new UserRankData()
                {
                    FullName = group.Key,
                    TotalCount = weekResumesByGroup.Count,
                    QualifiedCount = weekResumesByGroup.Count(c => c.AuditStatus == AuditStatus.Complete && c.Enable),
                    Photo = photo
                });
                // 天
                var dayResumesByGroup = mothlyResumes.Where(w => w.CreatorUserName == group.Key && w.CreationTime >= startTime && w.CreationTime < endTime).ToList();
                resumeRankData.DayUserRanks.Add(new UserRankData()
                {
                    FullName = group.Key,
                    TotalCount = dayResumesByGroup.Count,
                    QualifiedCount = dayResumesByGroup.Count(c => c.AuditStatus == AuditStatus.Complete && c.Enable),
                    Photo = photo
                });

            }

            model.ResumeRankData = resumeRankData;

            // 调查排行榜
            var investigationRankData = new RankData()
            {
                MonthlyUserRanks = new List<UserRankData>(),
                WeekUserRanks = new List<UserRankData>(),
                DayUserRanks = new List<UserRankData>()
            };
            foreach (var group in handledNameInvestigationGroups)
            {
                string photo = string.Empty;
                if (group.First() != null)
                {
                    photo = group.First()?.OwnerUserPhoto;
                }
                // 月
                var mothlyInvestigationsByGroup = monthlyInvestigations.Where(w => w.OwnerUserName == group.Key).ToList();
                investigationRankData.MonthlyUserRanks.Add(new UserRankData()
                {
                    FullName = group.Key,
                    TotalCount = mothlyInvestigationsByGroup.Count,
                    QualifiedCount = mothlyInvestigationsByGroup.Count(c => c.AcceptTravelStatus == AcceptTravelStatus.Accept),
                    Photo = photo
                });


                // 周
                var weekInvestigationsByGroup = monthlyInvestigations.Where(w => w.OwnerUserName == group.Key && w.CreationTime >= thisWeekStartTime && w.CreationTime < thisWeekEndTime).ToList();
                investigationRankData.WeekUserRanks.Add(new UserRankData()
                {
                    FullName = group.Key,
                    TotalCount = weekInvestigationsByGroup.Count,
                    QualifiedCount = weekInvestigationsByGroup.Count(c => c.AcceptTravelStatus == AcceptTravelStatus.Accept),
                    Photo = photo
                });
                // 天
                var dayInvestigationsByGroup = monthlyInvestigations.Where(w => w.OwnerUserName == group.Key && w.CreationTime >= startTime && w.CreationTime < endTime).ToList();
                investigationRankData.DayUserRanks.Add(new UserRankData()
                {
                    FullName = group.Key,
                    TotalCount = dayInvestigationsByGroup.Count,
                    QualifiedCount = dayInvestigationsByGroup.Count(c => c.AcceptTravelStatus == AcceptTravelStatus.Accept),
                    Photo = photo
                });

            }
            model.InvestigaionRankData = investigationRankData;

            #endregion

            #region 待处理任务
            if (onlyMyself)
            {
                var todoTasks = await _resumeQuerier.GetUncompleteResumesAsync(userId);
                model.TodoTasks = todoTasks;
            }
            else
            {
                var todoTasks = await _resumeQuerier.GetUncompleteResumesAsync(null);
                model.TodoTasks = todoTasks;
            }
            #endregion

            #region 预约中
            if (onlyMyself)
            {
                var todoTasks = await _interviewQuerier.GetUnfinshInterviewsAsync(userId);
                model.InterviewTasks = todoTasks;
            }
            else
            {
                var todoTasks = await _interviewQuerier.GetUnfinshInterviewsAsync(null);
                model.InterviewTasks = todoTasks;
            }

            #endregion

            return View(model);
        }
    }
}
