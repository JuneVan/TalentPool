using System.Collections.Generic;
using Van.TalentPool.Navigations;
using Van.TalentPool.Permissions;

namespace Van.TalentPool.Web.Auth
{
    public class Pages
    {
        public const string Authorization = "Pages.Authorization";

        public const string Authorization_Role = "Pages.Authorization.Role";
        public const string Authorization_Role_CreateOrEditOrDelete = "Pages.Authorization.Role.CreateOrEditOrDelete";
        public const string Authorization_Role_AssignPermission = "Pages.Authorization.Role.AssignPermission";


        public const string Authorization_User = "Pages.Authorization.User";
        public const string Authorization_User_CreateOrEditOrDelete = "Pages.Authorization.User.CreateOrEditOrDelete";
        public const string Authorization_User_AssignRole = "Pages.Authorization.User.AssignRole";

        public const string Configuration = "Pages.Configuration";
        public const string Configuration_SiteSetting = "Pages.Configuration.SiteSetting";
        public const string Configuration_UserSetting = "Pages.Configuration.UserSetting";
        public const string Configuration_EmailSetting = "Pages.Configuration.EmailSetting";
        public const string Configuration_Dictionary = "Pages.Configuration.Dictionary";
        public const string Configuration_Dictionary_CreateOrEditOrDelete = "Pages.Configuration.Dictionary.CreateOrEditOrDelete";

        public const string Job = "Pages.Job";
        public const string Job_CreateOrEditOrDelete = "Pages.Job.CreateOrEditOrDelete";

        public const string Resume = "Pages.Resume";
        public const string Resume_CreateOrEditOrDelete = "Pages.Resume.CreateOrEditOrDelete";
        public const string Resume_AssignUser = "Pages.Resume.AssignUser";
        public const string Resume_Audit = "Pages.Resume.AssignUser";
        public const string Resume_SendEmail = "Pages.Resume.SendEmail";
        public const string Resume_AuditSetting = "Pages.Resume.AuditSetting";

        public const string Investigation = "Pages.Investigation";
        public const string Investigation_CreateOrEditOrDelete = "Pages.Investigation.CreateOrEditOrDelete";
        public const string Investigation_FinshOrRestore = "Pages.Investigation.FinshOrRestore";
        public const string Investigation_Audit = "Pages.Investigation.Audit";

        public const string Interview = "Pages.Interview";
        public const string Interview_CreateOrEditOrDelete = "Pages.Interview.CreateOrEditOrDelete";

        public const string Evaluation = "Pages.Evaluation";
        public const string Evaluation_CreateOrEditOrDelete = "Pages.Evaluation.CreateOrEditOrDelete";

        public const string DailyStatistic = "Pages.DailyStatistic";
        public const string DailyStatistic_CreateOrEditOrDelete = "Pages.DailyStatistic.CreateOrEditOrDelete";



    }
    public class StandardPermissionProvider : IPermissionProvider
    {
        public IEnumerable<PermissionDefinition> Definitions()
        {
            var authorization = new PermissionDefinition(Pages.Authorization, "授权管理");

            var roles = authorization.AddChild(Pages.Authorization_Role, "角色管理", "拥有管理角色的权限。");
            roles.AddChild(Pages.Authorization_Role_CreateOrEditOrDelete, "创建、编辑、删除角色");
            roles.AddChild(Pages.Authorization_Role_AssignPermission, "分配权限");

            var users = authorization.AddChild(Pages.Authorization_User, "用户管理", "拥有管理用户的权限。");
            users.AddChild(Pages.Authorization_User_CreateOrEditOrDelete, "创建、编辑、删除用户");
            users.AddChild(Pages.Authorization_User_AssignRole, "分配角色");

            var configuration = new PermissionDefinition(Pages.Configuration, "系统配置", "拥有系统配置的管理权限。");
            configuration.AddChild(Pages.Configuration_SiteSetting, "站点配置");
            configuration.AddChild(Pages.Configuration_UserSetting, "账户配置");
            configuration.AddChild(Pages.Configuration_EmailSetting, "邮件配置");

            var dictionary = configuration.AddChild(Pages.Configuration_Dictionary, "数据字典");
            dictionary.AddChild(Pages.Configuration_Dictionary_CreateOrEditOrDelete, "创建、编辑、删除数据字典");

            var job = new PermissionDefinition(Pages.Job, "职位", "拥有职位的管理权限。");
            job.AddChild(Pages.Job_CreateOrEditOrDelete, "创建、编辑、删除职位");

            var resume = new PermissionDefinition(Pages.Resume, "简历管理", "拥有简历库的管理权限。");
            resume.AddChild(Pages.Resume_CreateOrEditOrDelete, "创建、编辑、删除简历");
            resume.AddChild(Pages.Resume_AssignUser, "分配负责人");
            resume.AddChild(Pages.Resume_SendEmail, "发送邮件");
            resume.AddChild(Pages.Resume_AuditSetting, "审核配置");

            var investigation = new PermissionDefinition(Pages.Investigation, "意向调查", "拥有意向调查的管理权限。");
            investigation.AddChild(Pages.Investigation_CreateOrEditOrDelete, "创建、编辑、删除意向调查");
            investigation.AddChild(Pages.Investigation_FinshOrRestore, "结束调查/恢复调查");
            investigation.AddChild(Pages.Investigation_Audit, "审核调查");

            var interview = new PermissionDefinition(Pages.Interview, "面试预约", "拥有面试预约的管理权限。");
            interview.AddChild(Pages.Interview_CreateOrEditOrDelete, "创建、编辑、删除面试预约");

            var evaluation = new PermissionDefinition(Pages.Evaluation, "技术评测", "拥有技术评测的权限。");
            evaluation.AddChild(Pages.Evaluation_CreateOrEditOrDelete, "创建、编辑、删除技术评测");

            var dailyStatistic = new PermissionDefinition(Pages.DailyStatistic, "每日数据统计", "拥有每日数据统计的权限。");
            dailyStatistic.AddChild(Pages.DailyStatistic_CreateOrEditOrDelete, "创建、编辑、删除每日数据统计");

            return new[] {
            authorization,
            configuration,
            job,
            resume,
            investigation,
            interview,
            evaluation,
            dailyStatistic
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
            configuration.AddChild("数据字典", "/Dictionary/List", "fas fa-book", Pages.Configuration_Dictionary);
            configuration.AddChild("简历审核", "/Resume/AuditSetting", "fas fa-id-card", Pages.Resume_AuditSetting);

            var authorization = new NavigationDefinition("授权管理", string.Empty, "fas fa-shield-alt", Pages.Authorization);
            authorization.AddChild("角色管理", "/Role/List", "fas fa-user-secret", Pages.Authorization_Role);
            authorization.AddChild("用户管理", "/User/List", "fas fa-users", Pages.Authorization_User);

            var resume = new NavigationDefinition("简历库", "/Resume/List", "fas fa-id-card", Pages.Resume);
            var investigation = new NavigationDefinition("意向调查", "/Investigation/List", "fas fa-pen-alt", Pages.Investigation);
            var interview = new NavigationDefinition("面试预约", "/Interview/List", "fas fa-calendar-alt", Pages.Interview);
            var job = new NavigationDefinition("职位管理", "/Job/List", "fas fa-user-md", Pages.Job);
            var evaluation = new NavigationDefinition("技术评测", "/Evaluation/List", "fas fa-newspaper", Pages.Evaluation);
            var dailyStatistic = new NavigationDefinition("每日统计", "/DailyStatistic/List", "fas fa-chart-pie", Pages.DailyStatistic);

            return new[]
            {
                dashboard,
                resume,
                investigation,
                interview,
                job,
                evaluation,
                authorization,
                configuration,
                dailyStatistic
                };
        }
    }
}
