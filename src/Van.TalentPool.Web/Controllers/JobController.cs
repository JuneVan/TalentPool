using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Van.TalentPool.Application.Jobs;
using Van.TalentPool.Infrastructure.Notify;
using Van.TalentPool.Jobs;
using Van.TalentPool.Permissions;
using Van.TalentPool.Web.Auth;
using Van.TalentPool.Web.Models.CommonModels;
using Van.TalentPool.Web.Models.JobViewModels;

namespace Van.TalentPool.Web.Controllers
{
    [PermissionCheck(Pages.Job)]
    public class JobController : WebControllerBase
    {
        private readonly IJobQuerier _jobQuerier;
        private readonly JobManager _jobManager;
        public JobController(IJobQuerier jobQuerier,
            JobManager jobManager,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _jobQuerier = jobQuerier;
            _jobManager = jobManager;
        }
        public async Task<IActionResult> List(QueryJobInput input)
        {
            var output = await _jobQuerier.GetListAsync(input);
            return View(new PaginationModel<JobDto>(output, input));
        }

        [PermissionCheck(Pages.Job_CreateOrEditOrDelete)]
        public IActionResult Create()
        {
            return View();
        }
        [PermissionCheck(Pages.Job_CreateOrEditOrDelete)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrEditJobViewModel model)
        {
            if (ModelState.IsValid)
            {
                var job = Mapper.Map<Job>(model);
                await _jobManager.CreateAsync(job);
                Notifier.Success("你已成功创建了一条职位记录。");
                return RedirectToAction(nameof(List));
            }
            return View(model);
        }
        [PermissionCheck(Pages.Job_CreateOrEditOrDelete)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var job = await _jobManager.FindByIdAsync(id);
            if (job == null)
                NotFound(id);
            var model = Mapper.Map<CreateOrEditJobViewModel>(job);
            return View(model);
        }
        [PermissionCheck(Pages.Job_CreateOrEditOrDelete)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateOrEditJobViewModel model)
        {
            if (ModelState.IsValid)
            {
                var job = await _jobManager.FindByIdAsync(model.Id.Value);
                if (job == null)
                    NotFound(model.Id.Value);
                _ = Mapper.Map(model, job);
                await _jobManager.UpdateAsync(job);
                Notifier.Success("你已成功编辑了一条职位记录。");
                return RedirectToAction(nameof(List));
            }
            return View(model);
        }
        [PermissionCheck(Pages.Job_CreateOrEditOrDelete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var job = await _jobManager.FindByIdAsync(id);
            if (job == null)
                NotFound(id);
            var model = Mapper.Map<DeleteJobViewModel>(job);
            return View(model);
        }
        [PermissionCheck(Pages.Job_CreateOrEditOrDelete)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteJobViewModel model)
        {
            if (ModelState.IsValid)
            {
                var job = await _jobManager.FindByIdAsync(model.Id);
                if (job == null)
                    NotFound(model.Id);
                await _jobManager.DeleteAsync(job);
                Notifier.Success("你已成功删除了一条职位记录。");
                return RedirectToAction(nameof(List));
            }
            return View(model);
        }


        private IActionResult NotFound(Guid id)
        {
            Notifier.Warning($"未找到id:{id}的职位记录。");
            return RedirectToAction(nameof(List));
        }

    }
}
