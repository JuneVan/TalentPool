using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Une.TalentPool.Configurations;
using Une.TalentPool.Infrastructure.Imaging;
using Une.TalentPool.Infrastructure.Notify;
using Une.TalentPool.Permissions;
using Une.TalentPool.Web.Auth;
using Une.TalentPool.Web.Models.SettingViewModels;
using IOFile = System.IO.File;

namespace Une.TalentPool.Web.Controllers
{
    [PermissionCheck(Pages.Configuration)]
    public class SettingController : WebControllerBase
    {
        private readonly ConfigurationManager _configurationManager;
        private readonly IWebHostEnvironment _environment;
        public SettingController(IServiceProvider serviceProvider,
            ConfigurationManager configurationManager,
            IWebHostEnvironment environment)
            : base(serviceProvider)
        {
            _configurationManager = configurationManager;
            _environment = environment;
        }
        // 站点配置
        [PermissionCheck(Pages.Configuration_SiteSetting)]
        public async Task<IActionResult> SiteSetting()
        {
            var settings = await _configurationManager.GetSettingAsync<SiteSetting>();
            return View(Mapper.Map<SiteSettingViewModel>(settings));
        }
        [PermissionCheck(Pages.Configuration_SiteSetting)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SiteSetting(SiteSettingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var settings = Mapper.Map<SiteSetting>(model);

                if (Request.Form.Files != null && Request.Form.Files.Count > 0 && Request.Form.Files[0].Length > 0)
                {
                    try
                    {
                        var webRootPath = _environment.WebRootPath;
                        var imageDirPath = $"{webRootPath}/upload/logos";
                        if (!Directory.Exists(imageDirPath))
                            Directory.CreateDirectory(imageDirPath);

                        var fileName = $"{DateTime.Now:yyyyMMddHHmmssff}{ new Random().Next(10000, 99999) }.png";
                        //存储路径
                        var filePath = $"{imageDirPath}/{fileName}";

                        //上传文件
                        using (Stream stream = Request.Form.Files[0].OpenReadStream())
                        {
                            ImageHelper.Square(stream, filePath, 160, 160);
                        }

                        //删除旧的图片
                        var oldHeadImagePath = $"{webRootPath}{settings.Logo}";
                        if (IOFile.Exists(oldHeadImagePath))
                            IOFile.Delete(oldHeadImagePath);
                        settings.Logo = $"/upload/logos/{fileName}";//图片文件相对路径

                    }
                    catch
                    {
                        Notifier.Error("上传图片操作失败。");
                    }
                }
                await _configurationManager.SaveSettingAsync(settings);
                Notifier.Success("你已成功保存了站点配置。");
                return RedirectToAction(nameof(SiteSetting));
            }
            return View(model);
        }

        // 用户配置
        [PermissionCheck(Pages.Configuration_UserSetting)]
        public async Task<IActionResult> UserSetting()
        {
            var settings = await _configurationManager.GetSettingAsync<UserSetting>();
            return View(Mapper.Map<UserSettingViewModel>(settings));
        }
        [PermissionCheck(Pages.Configuration_UserSetting)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserSetting(UserSettingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var settings = Mapper.Map<UserSetting>(model);
                await _configurationManager.SaveSettingAsync(settings);
                Notifier.Success("你已成功保存了用户配置。");
                return RedirectToAction(nameof(UserSetting));
            }
            return View(model);
        }

        // 邮件配置
        [PermissionCheck(Pages.Configuration_EmailSetting)]
        public async Task<IActionResult> EmailSetting()
        {
            var settings = await _configurationManager.GetSettingAsync<EmailSetting>();
            return View(Mapper.Map<EmailSettingViewModel>(settings));
        }
        [PermissionCheck(Pages.Configuration_EmailSetting)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmailSetting(EmailSettingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var settings = Mapper.Map<EmailSetting>(model);
                await _configurationManager.SaveSettingAsync(settings);
                Notifier.Success("你已成功保存了邮件配置。");
                return RedirectToAction(nameof(EmailSetting));
            }
            return View(model);
        }

        // 用户自定义设置 
        public async Task<IActionResult> UserCustomSetting()
        {
            var settings = await _configurationManager.GetSettingAsync<UserCustomSetting>(UserIdentifier.UserId);
            return View(Mapper.Map<UserCustomSettingViewModel>(settings));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserCustomSetting(UserCustomSettingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var settings = Mapper.Map<UserCustomSetting>(model);
                await _configurationManager.SaveSettingAsync(settings, UserIdentifier.UserId);
                Notifier.Success("你已成功保存了个人配置。");
                return RedirectToAction(nameof(UserCustomSetting));
            }
            return View(model);
        }
    }
}
