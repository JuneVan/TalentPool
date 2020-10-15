using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Van.TalentPool.Application.Investigations;
using Van.TalentPool.Application.Resumes;
using Van.TalentPool.Investigations;
using Van.TalentPool.Resumes;
using Van.TalentPool.Web.Models.ReportViewModels;

namespace Van.TalentPool.Web.Controllers
{
    public class ReportController : WebControllerBase
    {
        private readonly IInvestigationQuerier _investigationQuerier;
        private readonly IResumeQuerier _resumeQuerier;
        public ReportController(IInvestigationQuerier investigationQuerier,
            IResumeQuerier resumeQuerier,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _investigationQuerier = investigationQuerier;
            _resumeQuerier = resumeQuerier;
        }
        [ResponseCache(Duration = 6 * 60)]
        public async Task<IActionResult> Output(DateTime date)
        {
            var model = new OuputViewModel();
            var startTime = date;
            var endTime = startTime.AddDays(1);

            model.Date = date.ToString("yyyy-MM-dd");
            // 调查记录
            var investigations = await _investigationQuerier.GetReportInvestigationsAsync(date);
            ; model.Investigations = investigations;


            ////平台数据统计 
            //var dailyStatistic = await _dailyStatisticsManager.DailyStatistics.Include(i => i.Details)
            //    .Where(w => w.Date == date)
            //    .ToListAsync();

            //foreach (var item in dailyStatistic)
            //{
            //    model.Summary += $"{item.Platform}：{ item.Remark}<br />";
            //}

            //今日新增简历
            var newResumes = await _resumeQuerier.GetStatisticResumesAsync(startTime, endTime, AuditStatus.Complete);
            //简历创建统计
            var perPersonResumes = newResumes.GroupBy(g => g.CreatorUserName)
               .ToList();
            var resumeStatisticInfo = new List<ResumeStatisticModel>();
            foreach (var perPersonResume in perPersonResumes)
            {
                var resumeStatisticModel = new ResumeStatisticModel()
                {
                    CreatorUserName = perPersonResume.Key,
                    Count = perPersonResume.Count()
                };
                resumeStatisticInfo.Add(resumeStatisticModel);
            }
            model.ResumeStatisticInfo = resumeStatisticInfo;


            //调查情况统计
            var groupInvestigations = investigations.GroupBy(g => g.OwnerUserName).ToList();
            model.InvestigationStatisticInfo = new List<InvestigationStatisticModel>();
            foreach (var groupInvestigation in groupInvestigations)
            {
                var handleInvestigations = investigations.Where(w => w.OwnerUserName == groupInvestigation.Key);

                var investigationStatisticDto = new InvestigationStatisticModel()
                {
                    Name = string.IsNullOrEmpty(groupInvestigation.Key) ? "未知" : groupInvestigation.Key,
                    TotalCount = handleInvestigations.Count()
                };
                investigationStatisticDto.AcceptCount = handleInvestigations.Count(w => w.AcceptTravelStatus == AcceptTravelStatus.Accept);
                investigationStatisticDto.RefuseCount = handleInvestigations.Count(w => w.AcceptTravelStatus == AcceptTravelStatus.Refuse);
                investigationStatisticDto.ConsiderCount = handleInvestigations.Count(w => w.AcceptTravelStatus == AcceptTravelStatus.Consider);
                investigationStatisticDto.MissedCount = handleInvestigations.Count(w => w.IsConnected == false);
                model.InvestigationStatisticInfo.Add(investigationStatisticDto);
            }

            return View(model);
        }
        public IActionResult SelectDate()
        {
            return View();
        }
    }
}
