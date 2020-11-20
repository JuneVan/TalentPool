using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalentPool.Application.Interviews;
using TalentPool.Application.Investigations;
using TalentPool.Application.Resumes;
using TalentPool.Infrastructure.Notify;
using TalentPool.Investigations;
using TalentPool.Resumes;
using TalentPool.Web.Models.IndexViewModels;

namespace TalentPool.Web.Controllers
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
        public async Task<IActionResult> Index()
        {

            var model = new IndexViewModel();
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
            // 今年
            var thisYearStartTime = new DateTime(DateTime.Now.Year, 1, 1);
            var thisYeasEndTime = thisYearStartTime.AddYears(1);
            #endregion


            #region 统计数据
            var monthlyIncreaseData = new MonthlyIncreaseData();
            var todayResumeData = new TodayResumeData();
            var todayInvestigationData = new TodayInvestigationData();

            // 简历
            var yearlyResumes = await _resumeQuerier.GetStatisticResumesAsync(thisYearStartTime, thisYeasEndTime, null);
            // 个人月简历总数量/月全部简历数量
            monthlyIncreaseData.NewResumeCount = yearlyResumes.Where(w => w.CreationTime >= thisMonthStartTime && w.CreationTime < thisMonthEndTime).Count(w => w.CreatorUserId == userId);
            monthlyIncreaseData.NewResumeTotalCount = yearlyResumes.Where(w => w.CreationTime >= thisMonthStartTime && w.CreationTime < thisMonthEndTime).Count();

            // 简历审核情况统计
            if (CustomSetting.DefaultOnlySeeMyselfData)
            {
                var todayResumes = yearlyResumes.Where(w => w.CreatorUserId == userId && w.CreationTime >= startTime && w.CreationTime <= endTime).ToList();
                todayResumeData.PassedCount = todayResumes.Count(w => w.AuditStatus == AuditStatus.Complete);
                todayResumeData.UnhandledCount = todayResumes.Count(w => w.AuditStatus == AuditStatus.NoStart || w.AuditStatus == AuditStatus.Ongoing);
                todayResumeData.UnpassedCount = todayResumes.Count - todayResumeData.PassedCount - todayResumeData.UnhandledCount;
            }
            else
            {
                var todayResumes = yearlyResumes.Where(w => w.CreationTime >= startTime && w.CreationTime <= endTime).ToList();
                todayResumeData.PassedCount = todayResumes.Count(w => w.AuditStatus == AuditStatus.Complete);
                todayResumeData.UnhandledCount = todayResumes.Count(w => w.AuditStatus == AuditStatus.NoStart || w.AuditStatus == AuditStatus.Ongoing);
                todayResumeData.UnpassedCount = todayResumes.Count - todayResumeData.PassedCount - todayResumeData.UnhandledCount;
            }




            // 调查
            var yearlyInvestigations = await _investigationQuerier.GetStatisticInvestigationsAsync(thisYearStartTime, thisYeasEndTime);
            // 个人月调查总数量/月全部调查数量
            monthlyIncreaseData.NewInvestigationCount = yearlyInvestigations.Where(w => w.CreationTime >= thisMonthStartTime && w.CreationTime < thisMonthEndTime).Count(w => w.OwnerUserId == userId);
            monthlyIncreaseData.NewInvestigationTotalCount = yearlyInvestigations.Where(w => w.CreationTime >= thisMonthStartTime && w.CreationTime < thisMonthEndTime).Count();

            // 调查情况统计
            if (CustomSetting.DefaultOnlySeeMyselfData)
            {
                var todayInvestigations = yearlyInvestigations.Where(w => w.OwnerUserId == userId && w.CreationTime >= startTime && w.CreationTime <= endTime).ToList();
                todayInvestigationData.AcceptCount = todayInvestigations.Count(w => w.AcceptTravelStatus == AcceptTravelStatus.Accept);
                todayInvestigationData.RefuseCount = todayInvestigations.Count(w => w.AcceptTravelStatus == AcceptTravelStatus.Refuse);
                todayInvestigationData.ConsiderCount = todayInvestigations.Count(w => w.AcceptTravelStatus == AcceptTravelStatus.Consider);
                todayInvestigationData.MissedCount = todayInvestigations.Count(w => w.IsConnected == false);
            }
            else
            {
                var todayInvestigations = yearlyInvestigations.Where(w => w.CreationTime >= startTime && w.CreationTime <= endTime).ToList();
                todayInvestigationData.AcceptCount = todayInvestigations.Count(w => w.AcceptTravelStatus == AcceptTravelStatus.Accept);
                todayInvestigationData.RefuseCount = todayInvestigations.Count(w => w.AcceptTravelStatus == AcceptTravelStatus.Refuse);
                todayInvestigationData.ConsiderCount = todayInvestigations.Count(w => w.AcceptTravelStatus == AcceptTravelStatus.Consider);
                todayInvestigationData.MissedCount = todayInvestigations.Count(w => w.IsConnected == false);
            }


            model.MonthlyIncreaseData = monthlyIncreaseData;
            model.TodayResumeData = todayResumeData;
            model.TodayInvestigationData = todayInvestigationData;

            // 预约
            var monthlyInterviews = await _interviewQuerier.GetStatisticInterviewsAsync(thisMonthStartTime, thisMonthEndTime);
            // 个人月预约总数量/月全部预约数量
            monthlyIncreaseData.NewInterviewCount = monthlyInterviews.Count(w => w.CreatorUserId == userId);
            monthlyIncreaseData.NewInterviewTotalCount = monthlyInterviews.Count;

            // 月简历统计图
            var monthlyResumes = yearlyResumes.Where(w => w.CreationTime > thisMonthStartTime && w.CreationTime <= thisMonthEndTime);
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
            var createdNameMonthlyResumesResumeGroups = monthlyResumes.GroupBy(g => g.CreatorUserName);

            foreach (var group in createdNameMonthlyResumesResumeGroups)
            {
                var resumesByGroup = monthlyResumes.Where(w => w.CreatorUserName == group.Key).ToList();
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
            var handledNameMonthlyInvestigations = yearlyInvestigations.Where(w => w.CreationTime > thisMonthStartTime && w.CreationTime <= thisMonthEndTime);
            var investigationMonthlyData = new MonthlyData()
            {
                Labels = new List<string>(),
                Datasets = new List<StatisticsModel>()
            };

            for (int i = 0; i < dayCount; i++)
            {
                investigationMonthlyData.Labels.Add(thisMonthStartTime.AddDays(i).ToString("yyyy-MM-dd"));
            }
            var handledNameMonthlyInvestigationGroups = handledNameMonthlyInvestigations.GroupBy(g => g.OwnerUserName);

            foreach (var group in handledNameMonthlyInvestigationGroups)
            {
                var investigationByGroup = handledNameMonthlyInvestigations.Where(w => w.OwnerUserName == group.Key).ToList();
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
                YearlyUserRanks = new List<UserRankData>(),
                MonthlyUserRanks = new List<UserRankData>(),
                WeekUserRanks = new List<UserRankData>(),
                DayUserRanks = new List<UserRankData>()
            };
            var creatorYearlyResumesResumeGroups = yearlyResumes.GroupBy(g => g.CreatorUserName);
            foreach (var group in creatorYearlyResumesResumeGroups)
            {
                string photo = string.Empty;
                if (group.First() != null)
                {
                    photo = group.First()?.CreatorUserPhoto;
                }
                //年
                var yearlyResumesByGroup = yearlyResumes.Where(w => w.CreatorUserName == group.Key).ToList();
                if (yearlyResumesByGroup.Count > 0)
                {
                    resumeRankData.YearlyUserRanks.Add(new UserRankData()
                    {
                        FullName = group.Key,
                        TotalCount = yearlyResumesByGroup.Count,
                        QualifiedCount = yearlyResumesByGroup.Count(c => c.AuditStatus == AuditStatus.Complete),
                        Photo = photo
                    });
                }

                // 月
                var mothlyResumesByGroup = yearlyResumes.Where(w => w.CreatorUserName == group.Key && w.CreationTime >= thisMonthStartTime && w.CreationTime < thisMonthEndTime).ToList();
                if (mothlyResumesByGroup.Count > 0)
                {
                    resumeRankData.MonthlyUserRanks.Add(new UserRankData()
                    {
                        FullName = group.Key,
                        TotalCount = mothlyResumesByGroup.Count,
                        QualifiedCount = mothlyResumesByGroup.Count(c => c.AuditStatus == AuditStatus.Complete),
                        Photo = photo
                    });
                }


                // 周
                var weekResumesByGroup = yearlyResumes.Where(w => w.CreatorUserName == group.Key && w.CreationTime >= thisWeekStartTime && w.CreationTime < thisWeekEndTime).ToList();
                if (weekResumesByGroup.Count > 0)
                {
                    resumeRankData.WeekUserRanks.Add(new UserRankData()
                    {
                        FullName = group.Key,
                        TotalCount = weekResumesByGroup.Count,
                        QualifiedCount = weekResumesByGroup.Count(c => c.AuditStatus == AuditStatus.Complete),
                        Photo = photo
                    });
                }

                // 天
                var dayResumesByGroup = yearlyResumes.Where(w => w.CreatorUserName == group.Key && w.CreationTime >= startTime && w.CreationTime < endTime).ToList();
                if (dayResumesByGroup.Count > 0)
                {
                    resumeRankData.DayUserRanks.Add(new UserRankData()
                    {
                        FullName = group.Key,
                        TotalCount = dayResumesByGroup.Count,
                        QualifiedCount = dayResumesByGroup.Count(c => c.AuditStatus == AuditStatus.Complete),
                        Photo = photo
                    });
                }


            }

            model.ResumeRankData = resumeRankData;

            // 调查排行榜
            var investigationRankData = new RankData()
            {
                YearlyUserRanks = new List<UserRankData>(),
                MonthlyUserRanks = new List<UserRankData>(),
                WeekUserRanks = new List<UserRankData>(),
                DayUserRanks = new List<UserRankData>()
            };
            var ownerYearlyInvestigationGroups = yearlyInvestigations.GroupBy(g => g.OwnerUserName);
            foreach (var group in ownerYearlyInvestigationGroups)
            {
                string photo = string.Empty;
                if (group.First() != null)
                {
                    photo = group.First()?.OwnerUserPhoto;
                }
                // 年
                var yearlyInvestigationsByGroup = yearlyInvestigations.Where(w => w.OwnerUserName == group.Key).ToList();
                if (yearlyInvestigationsByGroup.Count > 0)
                {
                    investigationRankData.YearlyUserRanks.Add(new UserRankData()
                    {
                        FullName = group.Key,
                        TotalCount = yearlyInvestigationsByGroup.Count,
                        QualifiedCount = yearlyInvestigationsByGroup.Count(c => c.AcceptTravelStatus == AcceptTravelStatus.Accept),
                        Photo = photo
                    });
                }

                // 月
                var mothlyInvestigationsByGroup = yearlyInvestigations.Where(w => w.OwnerUserName == group.Key && w.CreationTime >= thisMonthStartTime && w.CreationTime < thisMonthEndTime).ToList();
                if (mothlyInvestigationsByGroup.Count > 0)
                {
                    investigationRankData.MonthlyUserRanks.Add(new UserRankData()
                    {
                        FullName = group.Key,
                        TotalCount = mothlyInvestigationsByGroup.Count,
                        QualifiedCount = mothlyInvestigationsByGroup.Count(c => c.AcceptTravelStatus == AcceptTravelStatus.Accept),
                        Photo = photo
                    });
                }


                // 周
                var weekInvestigationsByGroup = yearlyInvestigations.Where(w => w.OwnerUserName == group.Key && w.CreationTime >= thisWeekStartTime && w.CreationTime < thisWeekEndTime).ToList();
                if (weekInvestigationsByGroup.Count > 0)
                {
                    investigationRankData.WeekUserRanks.Add(new UserRankData()
                    {
                        FullName = group.Key,
                        TotalCount = weekInvestigationsByGroup.Count,
                        QualifiedCount = weekInvestigationsByGroup.Count(c => c.AcceptTravelStatus == AcceptTravelStatus.Accept),
                        Photo = photo
                    });
                }
                // 天
                var dayInvestigationsByGroup = yearlyInvestigations.Where(w => w.OwnerUserName == group.Key && w.CreationTime >= startTime && w.CreationTime < endTime).ToList();
                if (dayInvestigationsByGroup.Count > 0)
                {
                    investigationRankData.DayUserRanks.Add(new UserRankData()
                    {
                        FullName = group.Key,
                        TotalCount = dayInvestigationsByGroup.Count,
                        QualifiedCount = dayInvestigationsByGroup.Count(c => c.AcceptTravelStatus == AcceptTravelStatus.Accept),
                        Photo = photo
                    });
                }

            }
            model.InvestigaionRankData = investigationRankData;

            #endregion

            #region 待处理任务
            if (CustomSetting.DefaultOnlySeeMyselfData)
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
            if (CustomSetting.DefaultOnlySeeMyselfData)
            {
                var todoTasks = await _interviewQuerier.GetUnfinshInterviewsAsync(userId);
                model.InterviewTasks = todoTasks;
            }
            else
            {
                var todoTasks = await _interviewQuerier.GetUnfinshInterviewsAsync(null);
                model.InterviewTasks = todoTasks;
            }
            // 过期预约
            var expriedInterviewCount = model.InterviewTasks.Count(w => w.AppointmentTime < DateTime.Now & w.CreatorUserId == userId);
            if (expriedInterviewCount > 0)
            {
                Notifier.Error($"你有{expriedInterviewCount}条面试预约记录超过预约时间，请及时处理。");
            }
            #endregion

            return View(model);
        }
    }
}
