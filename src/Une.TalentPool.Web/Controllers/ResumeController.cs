using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Une.TalentPool.Application.Dictionaries;
using Une.TalentPool.Application.Jobs;
using Une.TalentPool.Application.Resumes;
using Une.TalentPool.Application.Users;
using Une.TalentPool.Infrastructure.Extensions;
using Une.TalentPool.Infrastructure.Message.Email;
using Une.TalentPool.Infrastructure.Notify;
using Une.TalentPool.Jobs;
using Une.TalentPool.Permissions;
using Une.TalentPool.Resumes;
using Une.TalentPool.Web.Auth;
using Une.TalentPool.Web.Models.CommonModels;
using Une.TalentPool.Web.Models.JobViewModels;
using Une.TalentPool.Web.Models.ResumeViewModels;

namespace Une.TalentPool.Web.Controllers
{
    [PermissionCheck(Pages.Resume)]
    public class ResumeController : WebControllerBase
    {
        private readonly IResumeQuerier _resumeQuerier;
        private readonly IJobQuerier _jobQuerier;
        private readonly IDictionaryQuerier _dictionaryQuerier;
        private readonly IUserQuerier _userQuerier;
        private readonly JobManager _jobManager;
        private readonly ResumeManager _resumeManager;
        private readonly ResumeAuditSettingManager _resumeAuditSettingManager;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        public ResumeController(IResumeQuerier resumeQuerier,
            IJobQuerier jobQuerier,
            IDictionaryQuerier dictionaryQuerier,
            IUserQuerier userQuerier,
            JobManager jobManager,
            ResumeManager resumeManager,
            ResumeAuditSettingManager resumeAuditSettingManager,
            IWebHostEnvironment environment,
            IConfiguration configuration,
            IEmailSender emailSender,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _resumeQuerier = resumeQuerier;
            _dictionaryQuerier = dictionaryQuerier;
            _jobQuerier = jobQuerier;
            _jobManager = jobManager;
            _resumeManager = resumeManager;
            _resumeAuditSettingManager = resumeAuditSettingManager;
            _userQuerier = userQuerier;
            _emailSender = emailSender;
            _environment = environment;
            _configuration = configuration;
        }

        #region CURD
        public async Task<IActionResult> List(QueryResumeInput input)
        {
            var output = await _resumeQuerier.GetListAsync(input);

            var model = new QueryResumeViewModel();
            model.Output = new PaginationModel<ResumeDto>(output, input);
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

                try
                {
                    var resume = Mapper.Map<Resume>(model);
                    resume.OwnerUserId = UserIdentifier.UserId.Value;
                    resume.Enable = true;
                    await _resumeManager.CreateAsync(resume);
                    Notifier.Success("你已成功创建了一条简历记录。");
                    return RedirectToAction(nameof(Edit), new { resume.Id });
                }
                catch (Exception ex)
                {
                    Notifier.Warning(ex.Message);
                }
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
                try
                {
                    _ = Mapper.Map(model, resume);

                    resume.KeyMaps = new List<ResumeKeywordMap>();
                    if (!string.IsNullOrEmpty(model.Keywords))
                    {
                        var keywords = model.Keywords.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var keyword in keywords)
                        {
                            resume.KeyMaps.Add(new ResumeKeywordMap()
                            {
                                Keyword = keyword, 
                                Name = model.Name
                            });
                        }
                    }
                    resume = await _resumeManager.UpdateAsync(resume, model.IgnoreSimilarity);
                    Notifier.Success("你已成功编辑了一条简历记录。");
                    return RedirectToAction(nameof(List));
                }
                catch (Exception ex)
                {
                    Notifier.Warning(ex.Message);
                    model.ResumeCompares = resume.ResumeCompares
                        .Select(s => new ResumeCompareDto()
                        {
                            Similarity = s.Similarity,
                            RelationResumeName = s.RelationResumeName,
                            RelationResumeId = s.RelationResumeId
                        }).ToList();
                }

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
            var auditedRecords = await _resumeQuerier.GetResumeAuditRecordsAsync(id);
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
                        CreatorUserName = item.UserName
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

        #region 有效性

        public async Task<IActionResult> Trash(Guid id)
        {
            var resume = await _resumeManager.FindByIdAsync(id);
            if (resume == null)
                return NotFound(id);
            return View(Mapper.Map<TrashResumeViewModel>(resume));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Trash(TrashResumeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resume = await _resumeManager.FindByIdAsync(model.Id);
                if (resume == null)
                    return NotFound(model.Id);

                await _resumeManager.TrashAsync(resume, model.EnableReason);
                Notifier.Success($"你成功将{resume.Name}的简历设置为无效。");
                return RedirectToAction(nameof(List));
            }

            return View(model);
        }
        #endregion


        #region 面试邀请邮件 
        public async Task<IActionResult> SendEmail(Guid id)
        {

            var resume = await _resumeManager.FindByIdAsync(id);
            if (resume == null)
                return NotFound(id);

            var job = await _jobManager.FindByIdAsync(resume.JobId);
            var model = Mapper.Map<SendEmailViewModel>(resume);
            var templatePath = _configuration.GetValue<string>("EmailTemplates:JobDescription");
            var fileProvider = _environment.WebRootFileProvider;
            var fileInfo = fileProvider.GetFileInfo(templatePath);
            if (fileInfo.Exists)
            {
                using (var reader = new StreamReader(fileInfo.PhysicalPath))
                {
                    var template = reader.ReadToEnd();
                    template = template.Replace("$Name$", resume.Name);
                    template = template.Replace("$JobName$", job.Title);
                    template = template.Replace("$Requirements$", job.Requirements);
                    template = template.Replace("$Description$", job.Description);
                    model.Body = template;
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmail(SendEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _emailSender.SendEmailAsync(new EmailEntry()
                {
                    ToName = model.Name,
                    ToEmailAddress = model.Email,
                    Subject = model.Subject,
                    Body = model.Body
                });
                Notifier.Success($"你已经成功给{model.Name}发送了面试邀请！");
                return RedirectToAction(nameof(List));
            }
            return View(model);
        }
        #endregion


        #region 导出
        public async Task<IActionResult> Export(QueryExportResumeInput input)
        {
            var resumes = await _resumeQuerier.GetExportResumesAsync(input);
            var columnNames = new string[] {
                    "简历编号",
                    "来源",
                    "职位",
                    "简历",
                    "审核情况",
                    "姓名",
                    "电话",
                    "求职意向城市",
                };

            using (ExcelPackage package = new ExcelPackage())
            {

                var worksheet01 = package.Workbook.Worksheets.Add("简历列表");
                for (int i = 0; i < columnNames.Length; i++)
                {
                    worksheet01.Cells[1, i + 1].Value = columnNames[i];
                }
                worksheet01.Cells[1, 1, 1, 6].Style.Font.Bold = true;

                for (int i = 0; i < resumes.Count; i++)
                {
                    worksheet01.Cells[i + 2, 1].Value = resumes[i].PlatformName;
                    worksheet01.Cells[i + 2, 2].Value = resumes[i].JobName;
                    if (!string.IsNullOrEmpty(resumes[i].Description))
                    {
                        //添加简历记录
                        var name = !string.IsNullOrEmpty(resumes[i].Name) ? resumes[i].Name : i.ToString();
                        var resumeWorksheet = package.Workbook.Worksheets.Add($"简历{name}");
                        AddHtmlToWorksheet(resumes[i].Description, resumeWorksheet, "简历列表!A1");

                        worksheet01.Cells[i + 2, 4].Style.Font.UnderLine = true;
                        worksheet01.Cells[i + 2, 4].Style.Font.Color.SetColor(Color.Blue);
                        worksheet01.Cells[i + 2, 4].Hyperlink = new ExcelHyperLink($"简历{name}!A1", "简历");

                    }

                    worksheet01.Cells[i + 2, 5].Value = resumes[i].AuditStatus.GetDescription();
                    worksheet01.Cells[i + 2, 6].Value = resumes[i].Name;
                    worksheet01.Cells[i + 2, 7].Value = resumes[i].PhoneNumber;
                    worksheet01.Cells[i + 2, 8].Value = resumes[i].City;
                }
                return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "简历报告.xlsx");
            }
        }

        public static void AddHtmlToWorksheet(string htmlValue, ExcelWorksheet worksheet, string returnLink)
        {

            var replacement = htmlValue;
            replacement = new Regex("</span>|</td>|</button>|</a>|</font>").Replace(replacement, "{column}");// span td 替换成 列
            replacement = new Regex("</div>|</tr>|</p>|</li>|</ul>|</dt>|</dd>|</dl>|\\r\\n|</h[1-6]>").Replace(replacement, "{row}");// div tr p 替换成 行
            replacement = new Regex("<[^>]*?>").Replace(replacement, "");// 其他html标签清除 

            replacement = replacement.Replace("&nbsp;", "{column}");//空格替换成列
            var rows = replacement.Split("{row}", StringSplitOptions.RemoveEmptyEntries);
            var tables = new string[rows.Length][];
            //转换成行数组
            int maxRowCount = rows.Length;
            for (int i = 0; i < rows.Length; i++)
            {
                //转换成列数组
                var columns = rows[i].Split("{column}", StringSplitOptions.RemoveEmptyEntries);
                tables[i] = columns;
            }
            if (maxRowCount <= 0)
                return;
            worksheet.Cells[1, 1, maxRowCount, 20].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            worksheet.Cells[1, 1, maxRowCount, 20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[1, 1, maxRowCount, 20].IsRichText = true;
            worksheet.Cells[1, 1, maxRowCount, 20].Merge = true;

            for (int i = 0; i < tables.Length; i++)
            {
                if (tables[i] != null)
                {
                    string line = string.Join("", tables[i]);
                    worksheet.Cells[1, 1, maxRowCount, 20].RichText.Add(line, true);
                }
            }

            worksheet.Cells[1, 21].Style.Font.UnderLine = true;
            worksheet.Cells[1, 21].Style.Font.Color.SetColor(Color.Blue);
            worksheet.Cells[1, 21].Style.Font.Size = 14;
            worksheet.Cells[1, 21].Hyperlink = new ExcelHyperLink(returnLink, "返回");
        }
        #endregion
        private IActionResult NotFound(Guid id)
        {
            Notifier.Warning($"未找到id:{id}的简历记录。");
            return RedirectToAction(nameof(List));
        }
    }
}
