using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalentPool.Application;
using TalentPool.Application.DailyStatistics;
using TalentPool.Application.Dictionaries;
using TalentPool.Application.Jobs;
using TalentPool.AspNetCore.Mvc.Authorization;
using TalentPool.AspNetCore.Mvc.Notify;
using TalentPool.DailyStatistics;
using TalentPool.Resumes;
using TalentPool.Web.Auth;
using TalentPool.Web.Models.CommonModels;
using TalentPool.Web.Models.DailyStatisticViewModels;

namespace TalentPool.Web.Controllers
{
    [AuthorizeCheck(Pages.DailyStatistic)]
    public class DailyStatisticController : WebControllerBase
    {
        private readonly IDailyStatisticQuerier _dailyStatisticQuerier;
        private readonly IDictionaryQuerier _dictionaryQuerier;
        private readonly IJobQuerier _jobQuerier;
        private readonly DailyStatisticManager _dailyStatisticManager;
        public DailyStatisticController(IDailyStatisticQuerier dailyStatisticQuerier,
            IDictionaryQuerier dictionaryQuerier,
            IJobQuerier jobQuerier,
            DailyStatisticManager dailyStatisticManager,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _dailyStatisticQuerier = dailyStatisticQuerier;
            _dictionaryQuerier = dictionaryQuerier;
            _jobQuerier = jobQuerier;
            _dailyStatisticManager = dailyStatisticManager;
        }
        public async Task<IActionResult> List(PaginationInput input)
        {
            var output = await _dailyStatisticQuerier.GetListAsync(input);
            return View(new PaginationModel<DailyStatisticDto>(output, input));
        }
        [AuthorizeCheck(Pages.DailyStatistic_CreateOrEditOrDelete)]
        public async Task<IActionResult> Create()
        {
            return await BuildCreateOrEditDisplayAsync(null);
        }
        [AuthorizeCheck(Pages.DailyStatistic_CreateOrEditOrDelete)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrEditDailyStatisticViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dailyStatistic = Mapper.Map<DailyStatistic>(model);
                await _dailyStatisticManager.CreateAsync(dailyStatistic);
                Notifier.Success($"你已成功创建了“{model.Date}”的统计记录！");
                return RedirectToAction(nameof(List));
            }
            return await BuildCreateOrEditDisplayAsync(model);
        }
        [AuthorizeCheck(Pages.DailyStatistic_CreateOrEditOrDelete)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var dailyStatistic = await _dailyStatisticManager.FindByIdAsync(id);
            if (dailyStatistic == null)
                return NotFound(id);
            var model = Mapper.Map<CreateOrEditDailyStatisticViewModel>(dailyStatistic);
            return await BuildCreateOrEditDisplayAsync(model);
        }
        [AuthorizeCheck(Pages.DailyStatistic_CreateOrEditOrDelete)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateOrEditDailyStatisticViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dailyStatistic = await _dailyStatisticManager.FindByIdAsync(model.Id.Value);
                if (dailyStatistic == null)
                    return NotFound(model.Id.Value);

                _ = Mapper.Map(model, dailyStatistic);
                await _dailyStatisticManager.UpdateAsync(dailyStatistic);
                Notifier.Success($"你已成功编辑了“{model.Date}”的统计记录！");
                return RedirectToAction(nameof(List));
            }
            return await BuildCreateOrEditDisplayAsync(model);
        }
        [AuthorizeCheck(Pages.DailyStatistic_CreateOrEditOrDelete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dailyStatistic = await _dailyStatisticManager.FindByIdAsync(id);
            if (dailyStatistic == null)
                return NotFound(id);

            return View(Mapper.Map<DeleteDailyStatisticViewModel>(dailyStatistic));
        }
        [AuthorizeCheck(Pages.DailyStatistic_CreateOrEditOrDelete)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteDailyStatisticViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dailyStatistic = await _dailyStatisticManager.FindByIdAsync(model.Id);
                if (dailyStatistic == null)
                    return NotFound(model.Id);

                await _dailyStatisticManager.DeleteAsync(dailyStatistic);
                Notifier.Information($"你已成功删除了“{dailyStatistic.Date}”的统计记录！");
                return RedirectToAction(nameof(List));
            }
            return View(model);
        }


        private async Task<IActionResult> BuildCreateOrEditDisplayAsync(CreateOrEditDailyStatisticViewModel model)
        {
            if (model == null)
                model = new CreateOrEditDailyStatisticViewModel();
            var jobs = await _jobQuerier.GetJobsAsync();
            if (jobs != null)
            {
                model.Jobs = jobs.Select(s => new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = s.Title
                }).ToList();
            }
            var dictionaries = await _dictionaryQuerier.GetDictionaryAsync(ResumeDefaults.PlatformType);
            if (dictionaries != null)
            {
                model.Platforms = dictionaries.Select(s => new SelectListItem()
                {
                    Text = s.Name,
                    Value = s.Name
                }).ToList();
            }
            return View(model);
        }


        public IActionResult Chart()
        {
            return View();
        }

        public async Task<ActionResult<DailyStatisticChartViewModel>> GetChartData(DateTime startDate, DateTime endDate, string groupbyKey = "Platform")
        {
            var dayCount = (endDate - startDate).TotalDays;
            var model = new DailyStatisticChartViewModel
            { 
                //日期标签
                Labels = new List<string>()
            };
            for (int i = 0; i < dayCount; i++)
            {
                model.Labels.Add(startDate.AddDays(i).ToString("yyyy-MM-dd"));
            }

            var items = await _dailyStatisticQuerier.GetChartStatisticsAsync(startDate, endDate);
            if (groupbyKey == "Platform")
            {
                List<ChartItemModel> func(Func<DailyStatisticChartDto, string> groupBySelector, Func<DailyStatisticChartDto, int> sumSelector)
                {
                    var chartItems = items
                           .GroupBy(groupBySelector)
                           .Select(s => new ChartItemModel()
                           {
                               Label = s.Key
                           }).ToList();
                    foreach (var chartItem in chartItems)
                    {
                        var distributions = items.Where(w => w.Platform == chartItem.Label);
                        var values = new List<int>();
                        for (int i = 0; i < dayCount; i++)
                        {
                            DateTime s = startDate.AddDays(i), e = startDate.AddDays(i + 1);
                            var count = distributions.Where(w => w.Date >= s && w.Date < e).Sum(sumSelector);
                            values.Add(count);
                        }
                        chartItem.Values = values;
                    }

                    return chartItems;
                }
                model.UpdateData = func(f => f.Platform, s => s.UpdateCount);
                model.DownloadData = func(f => f.Platform, s => s.DownloadCount);
            }
            else
            {
                List<ChartItemModel> func(Func<DailyStatisticChartDto, string> groupBySelector, Func<DailyStatisticChartDto, int> sumSelector)
                {
                    var chartItems = items
                           .GroupBy(groupBySelector)
                           .Select(s => new ChartItemModel()
                           {
                               Label = s.Key
                           }).ToList();
                    foreach (var chartItem in chartItems)
                    {
                        var distributions = items.Where(w => w.JobName == chartItem.Label);
                        var values = new List<int>();
                        for (int i = 0; i < dayCount; i++)
                        {
                            DateTime s = startDate.AddDays(i), e = startDate.AddDays(i + 1);
                            var count = distributions.Where(w => w.Date >= s && w.Date < e).Sum(sumSelector);
                            values.Add(count);
                        }
                        chartItem.Values = values;
                    }

                    return chartItems;
                }
                model.UpdateData = func(f => f.JobName, s => s.UpdateCount);
                model.DownloadData = func(f => f.JobName, s => s.DownloadCount);
            }
            return model;
        }

        private IActionResult NotFound(Guid id)
        {
            Notifier.Warning($"未找到id:“{id}”的统计记录。");
            return RedirectToAction(nameof(List));
        }
    }
}
