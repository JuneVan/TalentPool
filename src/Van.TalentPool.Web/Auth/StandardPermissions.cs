using System.Collections.Generic;
using Van.TalentPool.Navigations;
using Van.TalentPool.Permissions;

namespace Van.TalentPool.Web.Auth
{
    public class Pages
    {
        public const string Authorization = "Pages.Authorization";

        public const string Authorization_Role = "Pages.Authorization.Role";
        public const string Authorization_Role_Create = "Pages.Authorization.Role.Create";
        public const string Authorization_Role_Edit = "Pages.Authorization.Role.Edit";
        public const string Authorization_Role_AssignPermission = "Pages.Authorization.Role.AssignPermission";
        public const string Authorization_Role_Delete = "Pages.Authorization.Role.Delete";


        public const string Authorization_User = "Pages.Authorization.User";
        public const string Authorization_User_Create = "Pages.Authorization.User.Create";
        public const string Authorization_User_Edit = "Pages.Authorization.User.Edit";
        public const string Authorization_User_AssignRole = "Pages.Authorization.User.AssignRole";
        public const string Authorization_User_Delete = "Pages.Authorization.User.Delete";

        public const string Configuration = "Pages.Configuration";
        public const string Configuration_SiteSetting = "Pages.Configuration.SiteSetting";
        public const string Configuration_UserSetting = "Pages.Configuration.UserSetting";
        public const string Configuration_EmailSetting = "Pages.Configuration.EmailSetting";

    }
    public class StandardPermissionProvider : IPermissionProvider
    {
        public IEnumerable<PermissionDefinition> Definitions()
        {
            var authorization = new PermissionDefinition(Pages.Authorization, "授权管理");

            var roles = authorization.AddChild(Pages.Authorization_Role, "角色管理", "拥有管理角色的权限。");
            roles.AddChild(Pages.Authorization_Role_Create, "创建角色");
            roles.AddChild(Pages.Authorization_Role_Edit, "编辑角色");
            roles.AddChild(Pages.Authorization_Role_AssignPermission, "分配权限");
            roles.AddChild(Pages.Authorization_Role_Delete, "删除角色");

            var users = authorization.AddChild(Pages.Authorization_User, "用户管理", "拥有管理用户的权限。");
            users.AddChild(Pages.Authorization_User_Create, "创建用户");
            users.AddChild(Pages.Authorization_User_Edit, "编辑用户");
            users.AddChild(Pages.Authorization_User_AssignRole, "分配角色");
            users.AddChild(Pages.Authorization_User_Delete, "删除用户");

            var configuration = new PermissionDefinition(Pages.Configuration, "系统配置", "拥有系统配置的管理权限。");
            configuration.AddChild(Pages.Configuration_SiteSetting, "站点配置");
            configuration.AddChild(Pages.Configuration_UserSetting, "账户配置");
            configuration.AddChild(Pages.Configuration_EmailSetting, "邮件配置");

            return new[] {
            authorization,
            configuration
            };
        }
    }

    public class StandardNavigationProvider : INavigationProvider
    {
        public IEnumerable<NavigationDefinition> Definitions()
        {
            var dashboard = new NavigationDefinition("工作台", "/", "fas fa-tachometer-alt");

            var configuration = new NavigationDefinition("系统配置", string.Empty, "fas fa-cogs", Pages.Configuration);
            configuration.AddChild("站点配置", "/Setting/SiteSetting", "fas fa-book", Pages.Configuration_SiteSetting);
            configuration.AddChild("账户配置", "/Setting/UserSetting", "fas fa-user-cog", Pages.Configuration_UserSetting);
            configuration.AddChild("邮件配置", "/Setting/EmailSetting", "fas fa-envelope", Pages.Configuration_EmailSetting);

            var authorization = new NavigationDefinition("授权管理", string.Empty, "fas fa-shield-alt", Pages.Authorization);
            authorization.AddChild("角色管理", "/Role/List", "fas fa-user-secret", Pages.Authorization_Role);
            authorization.AddChild("用户管理", "/User/List", "fas fa-users", Pages.Authorization_User);

            return new[]
            {
                dashboard,
                authorization,
                configuration
                };
        }
    }
}
