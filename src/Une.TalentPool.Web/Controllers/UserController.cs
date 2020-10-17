using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Une.TalentPool.Application.Roles;
using Une.TalentPool.Application.Users;
using Une.TalentPool.Infrastructure.Imaging;
using Une.TalentPool.Infrastructure.Notify;
using Une.TalentPool.Permissions;
using Une.TalentPool.Users;
using Une.TalentPool.Web.Auth;
using Une.TalentPool.Web.Models.CommonModels;
using Une.TalentPool.Web.Models.UserViewModels;
using IOFile = System.IO.File;

namespace Une.TalentPool.Web.Controllers
{
    [PermissionCheck(Pages.Authorization_User)]
    public class UserController : WebControllerBase
    {
        private readonly UserManager _userManager;
        private readonly IRoleQuerier _roleQuerier;
        private readonly IWebHostEnvironment _environment;
        private readonly IUserQuerier _userQuerier;
        public UserController(IServiceProvider serviceProvider,
             IWebHostEnvironment environment,
            UserManager userManager,
           IRoleQuerier roleQuerier,
            IUserQuerier userQuerier)
            : base(serviceProvider)
        {
            _userManager = userManager;
            _environment = environment;
            _roleQuerier = roleQuerier;
            _userQuerier = userQuerier;
        }
        public async Task<IActionResult> List(QueryUserInput input)
        {
            var output = await _userQuerier.GetListAsync(input);
            return View(new PaginationModel<UserDto>(output, input));
        }

        // 创建用户

        [PermissionCheck(Pages.Authorization_User_CreateOrEditOrDelete)]
        public IActionResult Create()
        {
            return View();
        }

        [PermissionCheck(Pages.Authorization_User_CreateOrEditOrDelete)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrEditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = Mapper.Map<User>(model);
                // 上传头像
                _ = UploadProfile(user);
                // 设置默认密码
                if (string.IsNullOrEmpty(model.Password))
                    model.Password = UserDefaults.DefaultPassword;
                await _userManager.CreateAsync(user, model.Password);

                Notifier.Success("你已成功创建一个新的用户记录。");
                return RedirectToAction(nameof(List));

            }
            return View(model);
        }

        // 编辑用户

        [PermissionCheck(Pages.Authorization_User_CreateOrEditOrDelete)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound(id);

            var model = Mapper.Map<CreateOrEditUserViewModel>(user);
            return View(model);
        }

        [PermissionCheck(Pages.Authorization_User_CreateOrEditOrDelete)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateOrEditUserViewModel model)
        {

            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
                return NotFound(model.Id);

            if (ModelState.IsValid)
            {

                _ = Mapper.Map(model, user);
                // 上传头像
                _ = UploadProfile(user);
                await _userManager.UpdateAsync(user);
                Notifier.Success("你已成功编辑一条用户记录。");
                return RedirectToAction(nameof(List));
            }
            return View(model);
        }

        // 分配角色
        [PermissionCheck(Pages.Authorization_User_AssignRole)]
        public async Task<IActionResult> AssignRole(Guid id)
        {
            return await BuilAssginRoleDisplayAsync(id);
        }

        [PermissionCheck(Pages.Authorization_User_AssignRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(AssginRoleViewModel model)
        {

            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
                return NotFound(model.Id);


            if (ModelState.IsValid)
            {
                var roles = new List<string>();
                foreach (string key in Request.Form.Keys)
                {
                    if (key.StartsWith("Role.", StringComparison.Ordinal) && Request.Form[key] == "on")
                    {
                        string roleName = key.Substring("Role.".Length);
                        roles.Add(roleName);
                    }
                }
                await _userManager.UpdateRolesAsync(user, roles);
                Notifier.Success("你已成功编辑一条用户角色记录。");
                return RedirectToAction(nameof(List));
            }
            return await BuilAssginRoleDisplayAsync(model.Id);
        }
        private async Task<IActionResult> BuilAssginRoleDisplayAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound(id);

            var model = Mapper.Map<AssginRoleViewModel>(user);

            var roles = await _roleQuerier.GetRolesAsync();
            model.Roles = roles;
            if (user != null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);
                model.SelectedRoles = selectedRoles.ToList();
            }

            return View(model);
        }
        // 删除用户
        [PermissionCheck(Pages.Authorization_User_CreateOrEditOrDelete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound(id);
            var model = new DeleteUserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName
            };
            return View(model);
        }

        [PermissionCheck(Pages.Authorization_User_CreateOrEditOrDelete)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
                return NotFound(model.Id);
            await _userManager.DeleteAsync(user);
            Notifier.Success($"你成功删除了“{user.UserName}”的账户！");

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> View(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound(id);

            var model = Mapper.Map<UserViewModel>(user); 
            return View(model);
        }

         
        private User UploadProfile(User user)
        {
            /*
             
             图片路径可优化成绝对网络路径

             */
            if (Request.Form.Files != null && Request.Form.Files.Count > 0 && Request.Form.Files[0].Length > 0)
            {
                try
                {
                    var webRootPath = _environment.WebRootPath;
                    var imageDirPath = $"{webRootPath}/upload/user-photos";
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
                    var oldHeadImagePath = $"{webRootPath}/upload/user-photos/{user.Photo}";
                    if (IOFile.Exists(oldHeadImagePath))
                        IOFile.Delete(oldHeadImagePath);
                    user.Photo = $"/upload/user-photos/{fileName}";//图片文件相对路径

                }
                catch
                {
                    Notifier.Error("上传图片操作失败。");
                }
            }
            return user;
        }

        private IActionResult NotFound(Guid id)
        {
            Notifier.Warning($"未找到id:{id}的用户记录。");
            return RedirectToAction(nameof(List));
        }

    }
}
