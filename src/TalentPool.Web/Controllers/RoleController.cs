using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TalentPool.Application;
using TalentPool.Application.Roles;
using TalentPool.AspNetCore.Mvc.Authorization;
using TalentPool.AspNetCore.Mvc.Notify;
using TalentPool.Roles;
using TalentPool.Web.Auth;
using TalentPool.Web.Models.RoleViewModels;

namespace TalentPool.Web.Controllers
{
    public class RoleController : WebControllerBase
    {
        private readonly IRoleQuerier _roleQuerier;
        private readonly RoleManager _roleManager; 
        public RoleController(IRoleQuerier roleQuerier,
            RoleManager roleManager, 
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _roleQuerier = roleQuerier;
            _roleManager = roleManager; 

        }
        public async Task<IActionResult> List(PaginationInput input)
        {
            var output = await _roleQuerier.GetListAsync(input);
            return View(output);
        }

        // 创建角色

        [AuthorizeCheck(Pages.Authorization_Role_CreateOrEditOrDelete)]
        public IActionResult Create()
        {
            return View();
        }
        [AuthorizeCheck(Pages.Authorization_Role_CreateOrEditOrDelete)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrEditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = Mapper.Map<Role>(model);
                await _roleManager.CreateAsync(role);
                Notifier.Success("你已成功创建一条角色记录。");
                return RedirectToAction(nameof(List));
            }
            return View(model);
        }

        // 编辑角色

        [AuthorizeCheck(Pages.Authorization_Role_CreateOrEditOrDelete)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
                return NotFound(id);
            var model = Mapper.Map<CreateOrEditRoleViewModel>(role);
            return View(model);
        }
        [AuthorizeCheck(Pages.Authorization_Role_CreateOrEditOrDelete)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateOrEditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id.ToString());
            if (role == null)
                return NotFound(model.Id);
            if (ModelState.IsValid)
            {
                _ = Mapper.Map(model, role);
                await _roleManager.UpdateAsync(role);
                Notifier.Success("你已成功编辑一条角色记录。");
                return RedirectToAction(nameof(List));
            }
            return View(model);
        }


        // 编辑权限
        [AuthorizeCheck(Pages.Authorization_Role_AssignPermission)]
        public async Task<IActionResult> AssignPermission(Guid id)
        {
            return await BuildPermissionDisplayAsync(id);
        }
        [AuthorizeCheck(Pages.Authorization_Role_AssignPermission)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignPermission(AssignPermissionViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id.ToString());
            if (role == null)
                return NotFound(model.Id);
            if (ModelState.IsValid)
            {
                var permissions = new List<string>();
                foreach (string key in Request.Form.Keys)
                {
                    if (key.StartsWith("Permission.", StringComparison.Ordinal) && Request.Form[key] == "on")
                    {
                        string permissionName = key.Substring("Permission.".Length);
                        permissions.Add(permissionName);
                    }
                }
                await _roleManager.UpdatePermissionsAsync(role, permissions);

                Notifier.Success("你已成功编辑一条角色权限记录。");
                return RedirectToAction(nameof(List));
            }
            return await BuildPermissionDisplayAsync(model.Id);
        }

        private async Task<IActionResult> BuildPermissionDisplayAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
                return NotFound(id);
            var model = Mapper.Map<AssignPermissionViewModel>(role);
            model.Permissions = await _roleManager.GetPermissionsAsync(role); 

            return View(model);

        }


        [AuthorizeCheck(Pages.Authorization_Role_CreateOrEditOrDelete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
                return NotFound(id);
            return View(Mapper.Map<DeleteRoleViewModel>(role));
        }
        [AuthorizeCheck(Pages.Authorization_Role_CreateOrEditOrDelete)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id.ToString());
            if (role == null)
                return NotFound(model.Id);
            await _roleManager.DeleteAsync(role);
            Notifier.Success("你已成功删除一条角色记录。");
            return RedirectToAction(nameof(List));
        }

        private IActionResult NotFound(Guid id)
        {
            Notifier.Warning($"未找到id:{id}的角色记录。");
            return RedirectToAction(nameof(List));
        }
    }
}
