using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TalentPool.Application.DailyStatistics;
using TalentPool.Application.Investigations;
using TalentPool.Application.Resumes;
using TalentPool.Infrastructure.Extensions;
using TalentPool.Investigations;
using TalentPool.Resumes;
using TalentPool.Web.Models.ReportViewModels;

namespace TalentPool.Web.Controllers
{
    public class ReportController : WebControllerBase
    {
        private readonly IInvestigationQuerier _investigationQuerier;
        private readonly IResumeQuerier _resumeQuerier;
        private readonly IDailyStatisticQuerier _dailyStatisticQuerier;
        private readonly ILogger _logger;
        public ReportController(IInvestigationQuerier investigationQuerier,
            IResumeQuerier resumeQuerier,
            IDailyStatisticQuerier dailyStatisticQuerier,
            ILogger<ReportController> logger,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _investigationQuerier = investigationQuerier;
            _resumeQuerier = resumeQuerier;
            _dailyStatisticQuerier = dailyStatisticQuerier;
            _logger = logger;
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


            //平台数据统计 
            var dailyStatistics = await _dailyStatisticQuerier.GetDailyStatisticsAsync(date);
            foreach (var item in dailyStatistics)
            {
                model.Summary += $"{item.Platform}：{ item.Description}<br />";
            }

            //今日新增简历
            var newResumes = await _resumeQuerier.GetStatisticResumesAsync(startTime, endTime, AuditStatus.Complete);
            //简历创建统计
            var perPersonResumes = newResumes.GroupBy(g => g.CreatorUserName)
               .ToList();
            model.ResumeStatisticInfo = new List<ResumeStatisticModel>();
            foreach (var perPersonResume in perPersonResumes)
            {
                model.ResumeStatisticInfo.Add(new ResumeStatisticModel()
                {
                    CreatorUserName = perPersonResume.Key,
                    Count = perPersonResume.Count()
                });
            }

            // 职位统计
            var perJobResumes = newResumes.GroupBy(o => o.JobName);
            var deliveryCount = newResumes.Count(c => c.ActiveDelivery);
            model.JobStatisticTotalInfo = new JobStatisticTotalModel()
            {
                JobStatisticInfo = new List<JobStatisticModel>(),
                DeliveryCount = deliveryCount,
                SearchCount = newResumes.Count - deliveryCount
            };
            foreach (var item in perJobResumes)
            {
                model.JobStatisticTotalInfo.JobStatisticInfo.Add(new JobStatisticModel()
                {
                    JobName = item.Key,
                    Count = item.Count()
                });
            }


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

        public async Task<IActionResult> Export(DateTime date)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
             
            //今日调查人数(今日有更新的调查记录数)  
            var investigations = await _investigationQuerier.GetReportInvestigationsAsync(date);

            // 进行职位过滤处理
            var jobs = new List<string>();
            foreach (string key in Request.Form.Keys)
            {
                if (key.StartsWith("Job.", StringComparison.Ordinal) && Request.Form[key] == "on")
                {
                    string roleName = key.Substring("Job.".Length);
                    jobs.Add(roleName);
                }
            }
            var investigationList = new List<ReportInvestigationDto>(); 
            for (int i = 0; i < investigations.Count; i++)
            {
                // 过滤职位
                if (!jobs.Contains(investigations[i].JobName))
                    continue;

                investigationList.Add(investigations[i]);
            }

            #region 调查报表
            using (ExcelPackage package = new ExcelPackage())
            { 
                //02
                var worksheet01 = package.Workbook.Worksheets.Add("意向调查表");
                var columnNames2 = new string[] {
                    "时间",
                    "姓名",
                    "期望薪资" ,
                    "电话",
                    "职位",
                    "简历",
                    "意向调查",
                    "技术评测",
                    "是否接受出差",
                    "籍贯",
                    "来源",
                    "调查时间",
                    "调查人",
                    "现居住城市",
                    "是否现场面试",
                    "面试时间",
                    "是否合适"
                };
                for (int i = 0; i < columnNames2.Length; i++)
                {
                    worksheet01.Cells[1, i + 1].Value = columnNames2[i];
                }
                worksheet01.Cells[1, 1, 1, 17].Style.Font.Bold = true; 
                for (int i = 0; i < investigationList.Count; i++)
                {
                    try
                    {
                        worksheet01.Cells[i + 2, 1, i + 2, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        /** 颜色***/
                        //未开始调查状态(默认
                        if (investigationList[i].Status == InvestigationStatus.NoStart)
                        {
                            worksheet01.Cells[i + 2, 1, i + 2, 17].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                        }
                        else if (investigationList[i].Status == InvestigationStatus.Ongoing)
                        {
                            if (!investigationList[i].IsConnected.HasValue || !investigationList[i].IsConnected.Value)
                            {
                                worksheet01.Cells[i + 2, 1, i + 2, 17].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(108, 117, 125));//secondary
                            }
                            else //已接通 
                            {

                                if (investigationList[i].AcceptTravelStatus == AcceptTravelStatus.Consider)
                                {
                                    worksheet01.Cells[i + 2, 1, i + 2, 17].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 193, 7));//yellow
                                }
                                else
                                {
                                    worksheet01.Cells[i + 2, 1, i + 2, 17].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 123, 255));//blue
                                }
                            }
                        }
                        else
                        {
                            if (!investigationList[i].IsQualified.HasValue || !investigationList[i].IsQualified.Value)
                            {
                                //不合格
                                worksheet01.Cells[i + 2, 1, i + 2, 17].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                            }
                            else
                            {
                                worksheet01.Cells[i + 2, 1, i + 2, 17].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(40, 167, 69));//green
                            }
                        }
                        /** 颜色***/

                        worksheet01.Cells[i + 2, 1].Value = investigationList[i].CreationTime.ToString("yyyy-MM-dd");

                        if (!investigationList[i].IsConnected.HasValue || !investigationList[i].IsConnected.Value)//未接
                        {
                            worksheet01.Cells[i + 2, 2].Value = $"{investigationList[i].Name}(未接)";
                        }
                        else
                        {
                            worksheet01.Cells[i + 2, 2].Value = investigationList[i].Name;
                        }
                        worksheet01.Cells[i + 2, 3].Value = investigationList[i].ExpectedSalary;
                        worksheet01.Cells[i + 2, 4].Value = investigationList[i].PhoneNumber;
                        worksheet01.Cells[i + 2, 5].Value = investigationList[i].JobName;

                        string name = new Regex("[\u4e00-\u9fa5]{1,5}").Match(investigationList[i].Name).Value;
                        if (!string.IsNullOrEmpty(investigationList[i].Description))
                        {
                            //添加简历记录
                            var resumeWorksheet = package.Workbook.Worksheets.Add($"{name}简历");

                            AddHtmlToWorksheet(investigationList[i].Description, resumeWorksheet, "意向调查表!A1");


                            worksheet01.Cells[i + 2, 6].Style.Font.UnderLine = true;
                            worksheet01.Cells[i + 2, 6].Style.Font.Color.SetColor(Color.Blue);
                            worksheet01.Cells[i + 2, 6].Hyperlink = new ExcelHyperLink($"{name}简历!A1", "简历");

                        }
                        if (!string.IsNullOrEmpty(investigationList[i].Information))
                        {
                            //添加意向调查
                            var investigaionWorksheet = package.Workbook.Worksheets.Add($"{name}意向调查");

                            AddHtmlToWorksheet(investigationList[i].Information, investigaionWorksheet, "意向调查表!A1");

                            worksheet01.Cells[i + 2, 7].Style.Font.UnderLine = true;
                            worksheet01.Cells[i + 2, 7].Style.Font.Color.SetColor(Color.Blue);
                            worksheet01.Cells[i + 2, 7].Hyperlink = new ExcelHyperLink($"{name}意向调查!A1", "意向调查");


                        }
                        if (!string.IsNullOrEmpty(investigationList[i].Evaluation))
                        {
                            //添加技术评测
                            var evaluationWorksheet = package.Workbook.Worksheets.Add($"{name}技术评测");

                            AddHtmlToWorksheet(investigationList[i].Evaluation, evaluationWorksheet, "意向调查表!A1");

                            worksheet01.Cells[i + 2, 8].Style.Font.UnderLine = true;
                            worksheet01.Cells[i + 2, 8].Style.Font.Color.SetColor(Color.Blue);
                            worksheet01.Cells[i + 2, 8].Hyperlink = new ExcelHyperLink($"{name}技术评测!A1", "技术评测");


                        }

                        worksheet01.Cells[i + 2, 9].Value = investigationList[i].AcceptTravelStatus.GetDescription();
                        worksheet01.Cells[i + 2, 10].Value = investigationList[i].CityOfDomicile;
                        worksheet01.Cells[i + 2, 11].Value = investigationList[i].PlatformName;
                        worksheet01.Cells[i + 2, 12].Value = investigationList[i].InvestigateDate.ToString("yyyy-MM-dd");
                        worksheet01.Cells[i + 2, 13].Value = investigationList[i].OwnerUserName;
                        worksheet01.Cells[i + 2, 14].Value = investigationList[i].CityOfResidence;
                        worksheet01.Cells[i + 2, 15].Value = (investigationList[i].IsAcceptInterview.HasValue && investigationList[i].IsAcceptInterview.Value) ? "是" : "否";
                        worksheet01.Cells[i + 2, 16].Value = investigationList[i].ExpectedInterviewDate;
                        if (investigationList[i].IsQualified.HasValue && investigationList[i].IsQualified.Value)
                        {
                            worksheet01.Cells[i + 2, 17].Value = "合格";
                        }
                        else
                        {

                            if (!string.IsNullOrEmpty(investigationList[i].UnconnectedRemark))
                            {
                                worksheet01.Cells[i + 2, 17].Value = $"不合格({investigationList[i].UnconnectedRemark})";
                            }
                            else
                            {
                                worksheet01.Cells[i + 2, 17].Value = "不合格";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"导出报告异常[Name:{investigations[i].Name}]");
                    }
                }
                return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{date:yyyy-MM-dd} 意向调查报告.xlsx");

            }
            #endregion


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
    }
}
