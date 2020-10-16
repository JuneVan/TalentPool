using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;
using Van.TalentPool.Application;
using Van.TalentPool.Application.DailyStatistics;
using Van.TalentPool.Application.Dictionaries;
using Van.TalentPool.Application.Jobs;
using Van.TalentPool.DailyStatistics;
using Van.TalentPool.Infrastructure.Notify;
using Van.TalentPool.Permissions;
using Van.TalentPool.Resumes;
using Van.TalentPool.Web.Auth;
using Van.TalentPool.Web.Models.CommonModels;
using Van.TalentPool.Web.Models.DailyStatisticViewModels;

namespace Van.TalentPool.Web.Controllers
{
    [PermissionCheck(Pages.Authorization_Role_CreateOrEditOrDelete)]
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

        public async Task<IActionResult> Create()
        {
            return await BuildCreateOrEditDisplayAsync(null);
        }

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

        public async Task<IActionResult> Edit(Guid id)
        {
            var dailyStatistic = await _dailyStatisticManager.FindByIdAsync(id);
            if (dailyStatistic == null)
                return NotFound(id);
            var model = Mapper.Map<CreateOrEditDailyStatisticViewModel>(dailyStatistic);
            return await BuildCreateOrEditDisplayAsync(model);
        }

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

        public async Task<IActionResult> Delete(Guid id)
        {
            var dailyStatistic = await _dailyStatisticManager.FindByIdAsync(id);
            if (dailyStatistic == null)
                return NotFound(id);

            return View(Mapper.Map<DeleteDailyStatisticViewModel>(dailyStatistic));
        }

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

        private IActionResult NotFound(Guid id)
        {
            Notifier.Warning($"未找到id:“{id}”的统计记录。");
            return RedirectToAction(nameof(List));
        }
    }
}
