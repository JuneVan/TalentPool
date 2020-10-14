using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Van.TalentPool.Application.Dictionaries;
using Van.TalentPool.Application.Jobs;
using Van.TalentPool.Application.Resumes;
using Van.TalentPool.Application.Users;
using Van.TalentPool.Infrastructure.Notify;
using Van.TalentPool.Permissions;
using Van.TalentPool.Resumes;
using Van.TalentPool.Web.Auth;
using Van.TalentPool.Web.Models.CommonModels;
using Van.TalentPool.Web.Models.JobViewModels;
using Van.TalentPool.Web.Models.ResumeViewModels;

namespace Van.TalentPool.Web.Controllers
{
    [PermissionCheck(Pages.Resume)]
    public class ResumeController : WebControllerBase
    {
        private readonly IResumeQuerier _resumeQuerier;
        private readonly IJobQuerier _jobQuerier;
        private readonly IDictionaryQuerier _dictionaryQuerier;
        private readonly IUserQuerier _userQuerier;
        private readonly ResumeManager _resumeManager;
        private readonly ResumeAuditSettingManager _resumeAuditSettingManager;
        public ResumeController(IResumeQuerier resumeQuerier,
            IJobQuerier jobQuerier,
            IDictionaryQuerier dictionaryQuerier,
            IUserQuerier userQuerier,
            ResumeManager resumeManager,
            ResumeAuditSettingManager resumeAuditSettingManager,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _resumeQuerier = resumeQuerier;
            _dictionaryQuerier = dictionaryQuerier;
            _jobQuerier = jobQuerier;
            _resumeManager = resumeManager;
            _resumeAuditSettingManager = resumeAuditSettingManager;
            _userQuerier = userQuerier;
        }

        #region CURD
        public async Task<IActionResult> List(QueryResumeInput input)
        {
            var output = await _resumeQuerier.GetListAsync(input);

            var model = new QueryResumeViewModel();
            model.Pagination = new PaginationModel<ResumeDto>(output, input);
            return await BuildListDisplayAsync(model);
        }
        private async Task<IActionResult> BuildListDisplayAsync(QueryResumeViewModel model)
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
        public async Task<IActionResult> Create()
        {
            return await BuildCreateOrEditDisplayAsync(null);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrEditResumeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resume = Mapper.Map<Resume>(model);
                resume.OwnerUserId = UserIdentifier.UserId.Value;
                resume.Enable = true;
                await _resumeManager.CreateAsync(resume);
                Notifier.Success("你已成功创建了一条简历记录。");
                return RedirectToAction(nameof(Edit), new { resume.Id });
            }
            return await BuildCreateOrEditDisplayAsync(model);
        }
        // 编辑
        public async Task<IActionResult> Edit(Guid id)
        {
            var resume = await _resumeManager.FindByIdAsync(id);
            if (resume == null)
                return NotFound(id);
            var model = Mapper.Map<CreateOrEditResumeViewModel>(resume);
            return await BuildCreateOrEditDisplayAsync(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateOrEditResumeViewModel model)
        {
            var resume = await _resumeManager.FindByIdAsync(model.Id.Value);
            if (resume == null)
                return NotFound(model.Id);
            if (ModelState.IsValid)
            {
                _ = Mapper.Map(model, resume);

                resume.KeyMaps = new List<ResumeKeyMap>();
                var keywords = model.Keywords.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var keyword in keywords)
                {
                    resume.KeyMaps.Add(new ResumeKeyMap()
                    {
                        Keyword = keyword
                    });
                }
                await _resumeManager.UpdateAsync(resume);
                Notifier.Success("你已成功编辑了一条简历记录。");
                return RedirectToAction(nameof(List));
            }
            return await BuildCreateOrEditDisplayAsync(model);
        }

        private async Task<IActionResult> BuildCreateOrEditDisplayAsync(CreateOrEditResumeViewModel model)
        {
            if (model == null)
            {
                model = new CreateOrEditResumeViewModel()
                {
                    Jobs = new List<SelectListItem>()
                };
            }

            var jobs = await _jobQuerier.GetJobsAsync();
            if (jobs != null)
            {
                model.Jobs = jobs.Select(s => new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = s.Title
                }).ToList();
            }
            var platforms = await _dictionaryQuerier.GetDictionaryAsync(ResumeDefaults.PlatformType);
            if (platforms != null)
            {
                model.Platforms = platforms.Select(s => new SelectListItem()
                {
                    Value = s.Name,
                    Text = s.Name
                }).ToList();
            }
            return View(model);

        }



        // 删除
        public async Task<IActionResult> Delete(Guid id)
        {
            var resume = await _resumeManager.FindByIdAsync(id);
            if (resume == null)
                return NotFound(id);
            var model = Mapper.Map<DeleteResumeModel>(resume);

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteResumeModel model)
        {
            var resume = await _resumeManager.FindByIdAsync(model.Id);
            if (resume == null)
                return NotFound(model.Id);
            await _resumeManager.DeleteAsync(resume);

            Notifier.Success($"你已成功删除了一条简历信息！");
            return RedirectToAction(nameof(List));
        }
        // 查看简历
        public async Task<IActionResult> View(Guid id)
        {
            var resume = await _resumeQuerier.GetResumeAsync(id);
            if (resume == null)
                return NotFound(id);
            return View(resume);
        }
        #endregion


        #region 审核
        [PermissionCheck(Pages.Resume_Audit)]
        public async Task<IActionResult> Audit(Guid id)
        {
            var resume = await _resumeManager.FindByIdAsync(id);
            if (resume == null)
                return NotFound(id);

            var model = new CreateAuditViewModel();
            //审批记录
            var auditedRecords = await _resumeManager.GetAuditRecordsByResumeIdAsync(id);
            //审批设置
            var auditedSettings = await _resumeAuditSettingManager.GetAuditSettingsAsync();

            model.ResumeId = id;
            model.AuditRecords = new List<AuditRecordModel>();
            foreach (var item in auditedSettings.OrderBy(o => o.Order))
            {
                var auditRecord = auditedRecords.FirstOrDefault(w => w.CreatorUserId == item.UserId);

                if (auditRecord != null)
                {
                    model.AuditRecords.ForEach(f =>
                    {
                        if (!f.Passed.HasValue)
                        {
                            f.Remark = "已跳过该审核人员。";
                        }
                    });
                    var auditRecordModel = Mapper.Map<AuditRecordModel>(auditRecord);
                    model.AuditRecords.Add(auditRecordModel);
                    if (!auditRecord.Passed)
                        break;
                }
                else
                {
                    model.AuditRecords.Add(new AuditRecordModel()
                    {
                        CreatedName = item.UserName
                    });
                }
            }

            /* 
            审批检查
             */
            // ①当前用户是审批流程中的用户；
            var auditedSetting = auditedSettings.FirstOrDefault(f => f.UserId == UserIdentifier.UserId);
            if (auditedSetting == null)
                return View(model);
            // ②当前用户不存在审核记录；
            var auditedRecord = auditedRecords.FirstOrDefault(f => f.CreatorUserId == UserIdentifier.UserId);
            if (auditedRecord != null)
                return View(model);

            //  ③当前审批状态未完成
            if (resume.AuditStatus == AuditStatus.NoStart || resume.AuditStatus == AuditStatus.Ongoing)
                model.IsEnabled = true;

            return View(model);
        }
        [PermissionCheck(Pages.Resume_Audit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Audit(CreateAuditViewModel model)
        {
            var resume = await _resumeManager.FindByIdAsync(model.ResumeId);
            if (resume == null)
                return NotFound(model.ResumeId);

            var auditRecord = Mapper.Map<ResumeAuditRecord>(model);
            await _resumeManager.AuditAsync(resume, model.Passed, UserIdentifier.UserId.Value, auditRecord);
            Notifier.Success("你已成功提交了审核信息！");

            //通知负责人
            //if (resume.AuditStatus == AuditStatus.Complete)
            //{
            //    var notification = new NotifyEntry()
            //    {
            //        Content = $"我审核通过了一条简历记录，<a href=\"/Resume/View/{resume.Id}\">查看简历</a>",
            //        Receivers = new List<Guid>()
            //        {
            //            resume.CreatedBy
            //        }
            //    };
            //    await Notifier.NotifyAsync(notification);
            //}


            return RedirectToAction(nameof(List));
        }
        [PermissionCheck(Pages.Resume_Audit)]
        public async Task<IActionResult> CancelAudit(Guid auditRecordId)
        {
            var auditRecord = await _resumeManager.GetAuditRecordByIdAsync(auditRecordId);
            if (auditRecord == null)
                return NotFound();

            return View(new CancelAuditViewModel()
            {
                AuditRecordId = auditRecordId
            });
        }
        [PermissionCheck(Pages.Resume_Audit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelAudit(CancelAuditViewModel model)
        {

            var auditRecord = await _resumeManager.GetAuditRecordByIdAsync(model.AuditRecordId);
            if (auditRecord == null)
            {
                Notifier.Warning("当前审核记录不存在，无法执行取消操作。");
                return RedirectToAction(nameof(CancelAudit), new { model.AuditRecordId });
            }

            var resume = await _resumeManager.FindByIdAsync(auditRecord.ResumeId);
            if (resume == null)
                return NotFound(auditRecord.ResumeId);


            await _resumeManager.CancelAuditAsync(resume, UserIdentifier.UserId.Value, auditRecord);
            Notifier.Success("你已成功撤销了审核信息！");
            return RedirectToAction(nameof(Audit), new { Id = auditRecord.ResumeId });

        }
        [PermissionCheck(Pages.Resume_AuditSetting)]
        public async Task<IActionResult> AuditSetting()
        {

            var model = new AuditSettingViewModel();
            //读取简历的审批配置列表
            var settings = await _resumeAuditSettingManager.GetAuditSettingsAsync();
            model.AuditSettings = Mapper.Map<List<AuditSettingModel>>(settings);
            //读取管理员用户
            var users = await _userQuerier.GetUsersAsync();
            model.Users = Mapper.Map<List<AudtiSettingUserModel>>(users);

            return View(model);
        }

        [PermissionCheck(Pages.Resume_AuditSetting)]
        [HttpPost]
        public async Task<IActionResult> AuditSetting(AuditSettingViewModel model)
        {
            var settings = Mapper.Map<List<ResumeAuditSetting>>(model.AuditSettings);
            //增加排序
            for (int i = 0; i < settings.Count; i++)
            {
                settings[i].Order = i;
            }
            await _resumeAuditSettingManager.SaveAuditSettingsAsync(settings);

            Notifier.Success("你已成功保存了审核设置！");

            return Ok();
        }

        #endregion

        #region 简历责任人分配
        [PermissionCheck(Pages.Resume_AssignUser)]
        public async Task<IActionResult> Assign(Guid id)
        { 
            var resume = await _resumeManager.FindByIdAsync(id);
            if (resume == null)
                return NotFound(id); 
            var model = Mapper.Map<AssignUserViewModel>(resume);  
            return await BuildAssignDisplayAsync(model);
        }
        [PermissionCheck(Pages.Resume_AssignUser)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(AssignUserViewModel model)
        {

            if (ModelState.IsValid)
            {
                var resume = await _resumeManager.FindByIdAsync(model.Id);
                if (resume == null)
                    return NotFound(model.Id);
                await _resumeManager.AssignUserAsync(resume, model.OwnerUserId);

                Notifier.Success("你已成功重新分配了简历！");

                ////通知负责人
                //var notification = new NotifyEntry()
                //{
                //    Content = $"我分配了一条简历记录给你，<a href=\"/Resume/View/{resume.Id}\">查看简历</a>",
                //    Receivers = new List<Guid>()
                //    {
                //         resume.HandledBy
                //    }
                //};
                //await Notifier.NotifyAsync(notification);

                return RedirectToAction(nameof(List));
            }
            return await BuildAssignDisplayAsync(model); 
        }
        private async Task<IActionResult> BuildAssignDisplayAsync(AssignUserViewModel model)
        {
            var users = await _userQuerier.GetUsersAsync();
            model.Users = users.Select(s => new SelectListItem()
            {
                Text = s.FullName,
                Value = s.Id.ToString()
            }).ToList();

            return View(model);
        }

        #endregion


        private IActionResult NotFound(Guid id)
        {
            Notifier.Warning($"未找到id:{id}的简历记录。");
            return RedirectToAction(nameof(List));
        }
    }
}
