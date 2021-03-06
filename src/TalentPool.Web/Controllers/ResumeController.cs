﻿using Microsoft.AspNetCore.Hosting;
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
using TalentPool.Application.Dictionaries;
using TalentPool.Application.Jobs;
using TalentPool.Application.Resumes;
using TalentPool.Application.Users;
using TalentPool.AspNetCore.Mvc.Authorization;
using TalentPool.AspNetCore.Mvc.Notify;
using TalentPool.Configurations;
using TalentPool.Infrastructure.Extensions;
using TalentPool.Infrastructure.Message.Email;
using TalentPool.Jobs;
using TalentPool.Resumes;
using TalentPool.Web.Auth;
using TalentPool.Web.Models.CommonModels;
using TalentPool.Web.Models.ResumeViewModels;
using IOFile = System.IO.File;

namespace TalentPool.Web.Controllers
{
    [AuthorizeCheck(Pages.Resume)]
    public class ResumeController : WebControllerBase
    {
        private readonly IResumeQuerier _resumeQuerier;
        private readonly IJobQuerier _jobQuerier;
        private readonly IDictionaryQuerier _dictionaryQuerier;
        private readonly IUserQuerier _userQuerier;
        private readonly JobManager _jobManager;
        private readonly ResumeManager _resumeManager;
        private readonly ResumeAuditSettingManager _resumeAuditSettingManager;
        private readonly ConfigurationManager _configurationManager;
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
            ConfigurationManager configurationManager,
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
            _configurationManager = configurationManager;
            _userQuerier = userQuerier;
            _emailSender = emailSender;
            _environment = environment;
            _configuration = configuration;
            InitResumeSetting();
             
        }
        //初始化简历配置
        private async void InitResumeSetting()
        {
            var resumeSetting = await _configurationManager.GetSettingAsync<ResumeSetting>();
            _resumeManager.Options.MinSimilarityValue = resumeSetting.MinSimilarityValue;
        }

        #region CURD
        public IActionResult Search(string keyword)
        {
            return RedirectToAction(nameof(List), new
            {
                StartTime = DateTime.Parse("2020-05-01"),
                EndTime = DateTime.Now.Date.AddDays(1),
                Keyword = keyword
            });
        }
        public async Task<IActionResult> List(QueryResumeInput input)
        {
            if (!input.OwnerUserId.HasValue)
            {
                if (CustomSetting.DefaultOnlySeeMyselfData)
                {
                    input.OwnerUserId = UserIdentifier.UserId;
                }
                else
                {
                    input.OwnerUserId = Guid.Empty;
                }
            }


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
        [AuthorizeCheck(Pages.Resume_CreateOrEditOrDelete)]
        public async Task<IActionResult> Create()
        {
            return await BuildCreateOrEditDisplayAsync(null);
        }
        [AuthorizeCheck(Pages.Resume_CreateOrEditOrDelete)]
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
        [AuthorizeCheck(Pages.Resume_CreateOrEditOrDelete)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var resume = await _resumeManager.FindByIdAsync(id);
            if (resume == null)
                return NotFound(id);
            var model = Mapper.Map<CreateOrEditResumeViewModel>(resume);
            return await BuildCreateOrEditDisplayAsync(model);
        }

        [AuthorizeCheck(Pages.Resume_CreateOrEditOrDelete)]
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
                    model.OwnerUserId = resume.OwnerUserId;
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
        [AuthorizeCheck(Pages.Resume_CreateOrEditOrDelete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var resume = await _resumeManager.FindByIdAsync(id);
            if (resume == null)
                return NotFound(id);
            var model = Mapper.Map<DeleteResumeModel>(resume);

            return View(model);
        }
        [AuthorizeCheck(Pages.Resume_CreateOrEditOrDelete)]
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

        #region 附件

        [AuthorizeCheck(Pages.Resume_CreateOrEditOrDelete)]
        public async Task<IActionResult> UploadAttachment(Guid id)
        {
            var resume = await _resumeManager.FindByIdAsync(id);
            if (resume == null)
                return NotFound(id);
            var model = Mapper.Map<UploadAttachmentViewModel>(resume);

            return View(model);
        }
        [AuthorizeCheck(Pages.Resume_CreateOrEditOrDelete)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadAttachment(UploadAttachmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resume = await _resumeManager.FindByIdAsync(model.Id);
                if (resume == null)
                    return NotFound(model.Id);

                if (Request.Form.Files != null && Request.Form.Files.Count > 0)
                {
                    try
                    {
                        var webRootPath = _environment.WebRootPath;
                        var dirPath = $"{webRootPath}/upload/resume-attachments";
                        if (!Directory.Exists(dirPath))
                            Directory.CreateDirectory(dirPath);

                        var attachments = new List<ResumeAttachment>();
                        foreach (var file in Request.Form.Files)
                        {
                            var oldFileName = file.FileName;
                            var fileExtensionName = oldFileName.Substring(oldFileName.LastIndexOf(".") + 1);
                            var fileName = $"{DateTime.Now:yyyyMMddHHmmssff}{ new Random().Next(10000, 99999) }.{fileExtensionName}";
                            //存储路径
                            var filePath = $"{dirPath}/{fileName}";
                            using (Stream stream = file.OpenReadStream())
                            {
                                using (FileStream fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                                {
                                    int size = 1024;
                                    byte[] buffer = new byte[size];
                                    int length;
                                    while ((length = stream.Read(buffer, 0, size)) > 0)
                                    {
                                        fileStream.Write(buffer);
                                    }
                                }
                            }
                            attachments.Add(new ResumeAttachment()
                            {
                                FileName = oldFileName,
                                FilePath = $"/upload/resume-attachments/{fileName}"
                            });
                        }
                        await _resumeManager.AddAttachmentAsync(resume, attachments);
                        Notifier.Success($"你已成功上传了{Request.Form.Files.Count}条简历附件记录。");
                        return RedirectToAction(nameof(UploadAttachment), new { Id = model.Id });
                    }
                    catch
                    {
                        Notifier.Error("上传附件操作失败。");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "请选择需要上传的附件文件。");
                }
            }
            return View(model);

        }

        [AuthorizeCheck(Pages.Resume_CreateOrEditOrDelete)]
        public async Task<IActionResult> RemoveAttachment(Guid id, Guid attachmentId)
        {
            var resume = await _resumeManager.FindByIdAsync(id);
            if (resume == null)
                return NotFound(id);
            var model = Mapper.Map<RemoveAttachmentViewModel>(resume);
            var attachment = resume.Attachments?.FirstOrDefault(f => f.Id == attachmentId);
            if (attachment == null)
                return NotFound();
            model.FileName = attachment.FileName;
            model.AttachmentId = attachment.Id;
            return View(model);
        }
        [AuthorizeCheck(Pages.Resume_CreateOrEditOrDelete)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAttachment(RemoveAttachmentViewModel model)
        {
            var resume = await _resumeManager.FindByIdAsync(model.Id);
            if (resume == null)
                return NotFound(model.Id);
            var attachment = resume.Attachments?.FirstOrDefault(f => f.Id == model.AttachmentId);
            if (attachment == null)
                return NotFound();
            // 删除物理文件
            var webRootPath = _environment.WebRootPath;
            var filePath = $"{webRootPath}/{attachment.FilePath}";
            if (IOFile.Exists(filePath))
                IOFile.Delete(filePath);
            await _resumeManager.RemoveAttachmentAsync(resume, attachment);

            Notifier.Success("你已成功删除了一条简历附件记录。");
            return RedirectToAction(nameof(UploadAttachment), new { Id = model.Id });
        }

        #endregion

        #region 审核
        [AuthorizeCheck(Pages.Resume_Audit)]
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
        [AuthorizeCheck(Pages.Resume_Audit)]
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
        [AuthorizeCheck(Pages.Resume_Audit)]
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
        [AuthorizeCheck(Pages.Resume_Audit)]
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
        [AuthorizeCheck(Pages.Resume_AuditSetting)]
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

        [AuthorizeCheck(Pages.Resume_AuditSetting)]
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
        [AuthorizeCheck(Pages.Resume_AssignUser)]
        public async Task<IActionResult> Assign(Guid id)
        {
            var resume = await _resumeManager.FindByIdAsync(id);
            if (resume == null)
                return NotFound(id);
            var model = Mapper.Map<AssignUserViewModel>(resume);
            return await BuildAssignDisplayAsync(model);
        }
        [AuthorizeCheck(Pages.Resume_AssignUser)]
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
                try
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
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
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
