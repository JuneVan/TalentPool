using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;
using Une.TalentPool.Application.Interviews;
using Une.TalentPool.Application.Jobs;
using Une.TalentPool.Application.Users;
using Une.TalentPool.Infrastructure.Notify;
using Une.TalentPool.Interviews;
using Une.TalentPool.Permissions;
using Une.TalentPool.Resumes;
using Une.TalentPool.Web.Auth;
using Une.TalentPool.Web.Models.CommonModels;
using Une.TalentPool.Web.Models.InterviewViewModels;

namespace Une.TalentPool.Web.Controllers
{
    [PermissionCheck(Pages.Interview)]
    public class InterviewController : WebControllerBase
    {
        private readonly IInterviewQuerier _interviewQuerier;
        private readonly IJobQuerier _jobQuerier;
        private readonly IUserQuerier _userQuerier;
        private readonly ResumeManager _resumeManager;
        private readonly InterviewManager _interviewManager;
        public InterviewController(
            IInterviewQuerier interviewQuerier,
            IJobQuerier jobQuerier,
            IUserQuerier userQuerier,
            ResumeManager resumeManager,
            InterviewManager interviewManager,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _interviewQuerier = interviewQuerier;
            _jobQuerier = jobQuerier;
            _userQuerier = userQuerier;
            _resumeManager = resumeManager;
            _interviewManager = interviewManager;
        }

        // 列表
        public async Task<IActionResult> List(QueryInterviewInput input)
        {
            var output = await _interviewQuerier.GetListAsync(input);
            var model = new QueryInterviewViewModel()
            {
                Output = new PaginationModel<InterviewDto>(output, input)
            };

            return await BuildListDisplayAsync(model);
        }

        private async Task<IActionResult> BuildListDisplayAsync(QueryInterviewViewModel model)
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

        // 创建 
        public async Task<IActionResult> Create(Guid id)
        {
            var resume = await _resumeManager.FindByIdAsync(id);
            if (resume == null)
                return NotFound(id);
            var model = Mapper.Map<CreateOrEditInterviewViewModel>(resume);
            return View(model);

        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrEditInterviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                var interview = Mapper.Map<Interview>(model);
                await _interviewManager.CreateAsync(interview);
                Notifier.Success($"你已成功创建了“{model.Name}”的预约记录！");

                return RedirectToAction(nameof(List));
            }
            return View(model);
        }

        // 编辑 
        public async Task<IActionResult> Edit(Guid id)
        { 
            var interview = await _interviewManager.FindByIdAsync(id);
            if (interview == null)
                return NotFound(id); 
            var model = Mapper.Map<CreateOrEditInterviewViewModel>(interview);
            return View(model);
        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateOrEditInterviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                var interview = await _interviewManager.FindByIdAsync(model.Id.Value);
                if (interview == null)
                    return NotFound(model.Id);
                interview = Mapper.Map(model, interview);
                await _interviewManager.UpdateAsync(interview); 
                Notifier.Success($"你已成功编辑了“{model.Name}”的预约记录！"); 
                return RedirectToAction(nameof(List));
            }
            return View(model);
        }
        // 取消预约
        public async Task<IActionResult> Cancel(Guid id)
        {
            var interview = await _interviewManager.FindByIdAsync(id);
            if (interview == null)
                return NotFound(id); 
            var model = Mapper.Map<CancelInterviewViewModel>(interview);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(CancelInterviewViewModel model)
        {

            if (ModelState.IsValid)
            {
                var interview = await _interviewManager.FindByIdAsync(model.Id);
                if (interview == null)
                    return NotFound(model.Id);
                await _interviewManager.CancelAsync(interview); 
                Notifier.Success($"你已成功取消了“{interview.Name}”的预约记录！"); 
                return RedirectToAction(nameof(List));
            }
            return View(model);
        }
        // 修改预约状态
        public async Task<IActionResult> Change(Guid id)
        {

            var interview = await _interviewManager.FindByIdAsync(id);
            if (interview == null)
                return NotFound(id);

            var model = Mapper.Map<ChangeInterviewViewModel>(interview);
            model.VisitedTime = DateTime.Now;

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Change(ChangeInterviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                var interview = await _interviewManager.FindByIdAsync(model.Id);
                if (interview == null)
                    return NotFound(model.Id);
                interview = Mapper.Map(model, interview); 
                await _interviewManager.ChangeAsync(interview); 
                Notifier.Success($"你已成功编辑了“{interview.Name}”的预约记录状态！"); 
                return RedirectToAction(nameof(List));
            }
            return View(model);
        }


        private IActionResult NotFound(Guid id)
        {
            Notifier.Warning($"未找到id:{id}的面试预约记录。");
            return RedirectToAction(nameof(List));
        }

    }
}
