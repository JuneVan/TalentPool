using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;
using Van.TalentPool.Application.Investigations;
using Van.TalentPool.Application.Jobs;
using Van.TalentPool.Application.Users;
using Van.TalentPool.Infrastructure.Notify;
using Van.TalentPool.Investigations;
using Van.TalentPool.Resumes;
using Van.TalentPool.Web.Models.CommonModels;
using Van.TalentPool.Web.Models.InvestigationViewModels;

namespace Van.TalentPool.Web.Controllers
{
    public class InvestigationController : WebControllerBase
    {
        private readonly IInvestigationQuerier _investigationQuerier;
        private readonly IJobQuerier _jobQuerier;
        private readonly IUserQuerier _userQuerier;
        private readonly ResumeManager _resumeManager;
        private readonly InvestigationManager _investigationManager;
        public InvestigationController(IServiceProvider serviceProvider,
            IInvestigationQuerier investigationQuerier,
            IJobQuerier jobQuerier,
            IUserQuerier userQuerier,
            ResumeManager resumeManager,
            InvestigationManager investigationManager)
            : base(serviceProvider)
        {
            _investigationQuerier = investigationQuerier;
            _jobQuerier = jobQuerier;
            _userQuerier = userQuerier;
            _resumeManager = resumeManager;
            _investigationManager = investigationManager;
        }
        public async Task<IActionResult> List(QueryInvestigaionInput input)
        {
            var output = await _investigationQuerier.GetListAsync(input);
            var model = new QueryInvestigationViewModel()
            {
                Pagination = new PaginationModel<InvestigationDto>(output, input)
            };
            return await BuildListDisplayAsync(model);
        }
        private async Task<IActionResult> BuildListDisplayAsync(QueryInvestigationViewModel model)
        {
            var jobs = await _jobQuerier.GetJobsAsync();
            if (jobs != null)
            {
                model.Jobs = jobs.Select(s => new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = s.Title
                }).ToList();
            }
            var users = await _userQuerier.GetUsersAsync();
            if (users != null)
            {
                model.Users = users.Select(s => new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = s.FullName
                }).ToList();
            }
            return View(model);
        }

        public async Task<IActionResult> Create(Guid id)
        {
            var resume = await _resumeManager.FindByIdAsync(id);
            if (resume == null)
                return NotFound(id);

            var model = Mapper.Map<CreateInvestigationViewModel>(resume);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInvestigationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var investigation = Mapper.Map<Investigation>(model);
                investigation.InvestigateDate = DateTime.Now.Date;
                await _investigationManager.CreateAsync(investigation);

                Notifier.Success($"你已成功创建了“{investigation.Name}”的意向调查记录！");
                return RedirectToAction(nameof(List));
            }
            return View(model);

        }

        private IActionResult NotFound(Guid id)
        {
            Notifier.Warning($"未找到id:“{id}”的调查记录。");
            return RedirectToAction(nameof(List));
        }
    }
}
