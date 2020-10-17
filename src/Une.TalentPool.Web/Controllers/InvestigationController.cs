using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Une.TalentPool.Application.Evaluations;
using Une.TalentPool.Application.Investigations;
using Une.TalentPool.Application.Jobs;
using Une.TalentPool.Application.Resumes;
using Une.TalentPool.Application.Users;
using Une.TalentPool.Infrastructure.Notify;
using Une.TalentPool.Investigations;
using Une.TalentPool.Permissions;
using Une.TalentPool.Resumes;
using Une.TalentPool.Web.Auth;
using Une.TalentPool.Web.Models.CommonModels;
using Une.TalentPool.Web.Models.InvestigationViewModels;

namespace Une.TalentPool.Web.Controllers
{
    public class InvestigationController : WebControllerBase
    {
        private readonly IInvestigationQuerier _investigationQuerier;
        private readonly IJobQuerier _jobQuerier;
        private readonly IUserQuerier _userQuerier;
        private readonly IResumeQuerier _resumeQuerier; 
        private readonly ResumeManager _resumeManager;
        private readonly InvestigationManager _investigationManager;
        public InvestigationController(IServiceProvider serviceProvider,
            IInvestigationQuerier investigationQuerier,
            IJobQuerier jobQuerier,
            IUserQuerier userQuerier,
            IResumeQuerier resumeQuerier, 
            ResumeManager resumeManager,
            InvestigationManager investigationManager)
            : base(serviceProvider)
        {
            _investigationQuerier = investigationQuerier;
            _jobQuerier = jobQuerier;
            _userQuerier = userQuerier;
            _resumeQuerier = resumeQuerier; 
            _resumeManager = resumeManager;
            _investigationManager = investigationManager;
        }
        #region CURD
        public async Task<IActionResult> List(QueryInvestigaionInput input)
        {
            var output = await _investigationQuerier.GetListAsync(input);
            var model = new QueryInvestigationViewModel()
            {
                Output = new PaginationModel<InvestigationDto>(output, input)
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

        public async Task<IActionResult> Edit(Guid id)
        {
            var investigation = await _investigationManager.FindByIdAsync(id);
            if (investigation == null)
                return NotFound(id);

            var model = Mapper.Map<EditInvestigationViewModel>(investigation);
            var resume = await _resumeQuerier.GetResumeAsync(investigation.ResumeId);
            model = Mapper.Map(resume, model);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditInvestigationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var investigation = await _investigationManager.FindByIdAsync(model.Id);
                if (investigation == null)
                    return NotFound(model.Id);
                investigation = Mapper.Map(model, investigation);
                investigation.Status = InvestigationStatus.Ongoing;
                if (investigation.IsConnected.HasValue && investigation.IsConnected.Value)
                    investigation.UnconnectedRemark = string.Empty;
                await _investigationManager.UpdateAsync(investigation);

                Notifier.Success($"你已成功编辑了“{investigation.Name}”的意向调查记录！");
                return RedirectToAction(nameof(List));
            }
            return View(model);
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            var investigation = await _investigationManager.FindByIdAsync(id);
            if (investigation == null)
                return NotFound(id);
            ViewBag.Id = id;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteInvestigationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var investigation = await _investigationManager.FindByIdAsync(model.Id);
                if (investigation == null)
                    return NotFound(model.Id);
                await _investigationManager.DeleteAsync(investigation);
                Notifier.Information($"你已成功删除了“{investigation.Name}”的意向调查记录！");
                return RedirectToAction(nameof(List));
            }
            return View(model);
        }
        public async Task<IActionResult> View(Guid id)
        {
            var investigation = await _investigationQuerier.GetInvestigationAsync(id);
            if (investigation == null)
                return NotFound(id);
            return View(investigation);
        }
        #endregion

        #region 审核
        [PermissionCheck(Pages.Investigation_Audit)]
        public async Task<IActionResult> Audit(Guid id)
        {

            var investigation = await _investigationManager.FindByIdAsync(id);
            if (investigation == null)
                return NotFound(id);
            var model = Mapper.Map<AuditInvestigationViewModel>(investigation);
            return View(model);

        }
        [PermissionCheck(Pages.Investigation_Audit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Audit(AuditInvestigationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var investigation = await _investigationManager.FindByIdAsync(model.Id);
                if (investigation == null)
                    return NotFound(model.Id);
                await _investigationManager.AuditAsync(investigation, model.IsQualified, model.QualifiedRemark);
                Notifier.Success($"你已成功审核了“{investigation.Name}”的意向调查记录！");

                //通知其处理人
                //var notification = new NotifyEntry()
                //{
                //    Content = $"我刚刚将你的一份关于“{investigation.Name}”的意向调查标记为{(model.IsQualified ? "合格" : "不合格")}，<a href=\"/Investigation/View/{investigation.Id}\">查看意向调查</a>"
                //};
                //notification.Receivers.Add(investigation.CreatedBy);
                //await Notifier.NotifyAsync(notification);

                return RedirectToAction(nameof(List));
            }
            return View(model);
        }
        #endregion

        #region 结束/恢复
        [PermissionCheck(Pages.Investigation_FinshOrRestore)]
        public async Task<IActionResult> Finsh(Guid id)
        {

            var investigation = await _investigationManager.FindByIdAsync(id);
            if (investigation == null)
                return NotFound(id);

            var model = Mapper.Map<FinshOrRestoreModel>(investigation);
            return View(model);
        }
        [PermissionCheck(Pages.Investigation_FinshOrRestore)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finsh(FinshOrRestoreModel model)
        {
            if (ModelState.IsValid)
            {
                var investigation = await _investigationManager.FindByIdAsync(model.Id);
                if (investigation == null)
                    return NotFound(model.Id);
                await _investigationManager.CompleteAsync(investigation);

                Notifier.Success($"你已成功完成了“{investigation.Name}”的意向调查记录！");

                ////通知其审批管理员
                //var auditUsers = await _resumeManager.ResumeAuditSettings.ToListAsync();
                //var notification = new NotifyEntry()
                //{
                //    Content = $"我完成了一份意向调查，<a href=\"/Investigation/View/{investigation.Id}\">查看意向调查</a>"
                //};
                //foreach (var auditUser in auditUsers)
                //{
                //    notification.Receivers.Add(auditUser.UserId);
                //}
                //await Notifier.NotifyAsync(notification);

                return RedirectToAction(nameof(List));
            }
            return View(model);
        }
        [PermissionCheck(Pages.Investigation_FinshOrRestore)]
        public async Task<IActionResult> Restore(Guid id)
        {

            var investigation = await _investigationManager.FindByIdAsync(id);
            if (investigation == null)
                return NotFound(id);

            var model = Mapper.Map<FinshOrRestoreModel>(investigation);
            return View(model);
        }
        [PermissionCheck(Pages.Investigation_FinshOrRestore)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(FinshOrRestoreModel model)
        {
            if (ModelState.IsValid)
            {
                var investigation = await _investigationManager.FindByIdAsync(model.Id);
                if (investigation == null)
                    return NotFound(model.Id);
                await _investigationManager.RestoreAsync(investigation);

                Notifier.Success($"你已成功恢复了“{investigation.Name}”的意向调查记录状态！");

                //通知其审批管理员
                //var auditUsers = await _resumeManager.ResumeAuditSettings.ToListAsync();
                //var notification = new NotifyEntry()
                //{
                //    Content = $"我重新恢复了一份意向调查，<a href=\"/Investigation/View/{investigation.Id}\">查看意向调查</a>"
                //};
                //foreach (var auditUser in auditUsers)
                //{
                //    if (auditUser.UserId == UserIdentifier.UserId)//不用通知自己
                //        continue;
                //    notification.Receivers.Add(auditUser.UserId);
                //}
                ////及创建者
                //notification.Receivers.Add(investigation.CreatedBy);
                //await Notifier.NotifyAsync(notification);
                return RedirectToAction(nameof(List));
            }
            return View(model);

        }
        #endregion


        [PermissionCheck(Pages.Investigation_CreateOrEditOrDelete)]
        [HttpPost]
        public async Task<IActionResult> Evaluate(EvaluateResultModel model)
        {

            if (model == null || model.Questions == null)
            {
                Notifier.Error("提交评测参数异常！");
                return BadRequest();
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<ol>");
            for (int i = 0; i < model.Questions.Count; i++)
            {
                stringBuilder.Append($"<li>【{model.Questions[i].Keyword}】{model.Questions[i].Description}评分：{model.Questions[i].Score}</li>");
            }
            stringBuilder.Append("</ol>");
            var investigation = await _investigationManager.FindByIdAsync(model.InvestigationId);
            if (investigation == null)
            {
                return NotFound(model.InvestigationId);
            }
            var evaluation = stringBuilder.ToString();
            await _investigationManager.EvaluateAsync(investigation, evaluation);

            Notifier.Success("你成功提交了技术测评结果！");


            ////通知审核管理员
            //var auditUsers = await _resumeManager.ResumeAuditSettings.ToListAsync();
            //var notification = new NotifyEntry()
            //{
            //    Content = $"我刚刚提交了关于“{investigation.Name}”的技术测评结果，<a href=\"/Investigation/View/{investigation.Id}\">查看意向调查</a>"
            //};
            //foreach (var auditUser in auditUsers)
            //{
            //    notification.Receivers.Add(auditUser.UserId);
            //}
            //await Notifier.NotifyAsync(notification);

            return Ok();
        }

        private IActionResult NotFound(Guid id)
        {
            Notifier.Warning($"未找到id:“{id}”的调查记录。");
            return RedirectToAction(nameof(List));
        }
    }
}
