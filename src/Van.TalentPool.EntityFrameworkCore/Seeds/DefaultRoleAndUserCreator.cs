using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Van.TalentPool.Roles;
using Van.TalentPool.Users;

namespace Van.TalentPool.EntityFrameworkCore.Seeds
{
    public class DefaultRoleAndUserCreator
    {
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;
        public DefaultRoleAndUserCreator(IServiceProvider serviceProvider)
        {
            _roleManager = serviceProvider.GetRequiredService<RoleManager>();
            _userManager = serviceProvider.GetRequiredService<UserManager>();
        }
        public async Task Create()
        {
            var adminRole = await _roleManager.FindByNameAsync(RoleDefaults.Administrators);
            if (adminRole == null)
            {
                adminRole = new Role()
                {
                    Name = RoleDefaults.Administrators,
                    DisplayName = "管理员",
                    Description = "拥有系统功能的全部权限",
                    Active = true,
                    Protected = true
                };
                await _roleManager.CreateAsync(adminRole);

                var claims = new Claim[]
                {
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Authorization"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Authorization.Role"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Authorization.Role.CreateOrEditOrDelete"), 
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Authorization.Role.AssignPermission"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Authorization.User"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Authorization.User.CreateOrEditOrDelete"), 
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Authorization.User.AssignRole"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Configuration"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Configuration.SiteSetting"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Configuration.UserSetting"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Configuration.EmailSetting"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Configuration.Dictionary"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Configuration.Dictionary.CreateOrEditOrDelete"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Job"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Job.CreateOrEditOrDelete"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Resume"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Resume.CreateOrEditOrDelete"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Resume.AssignUser"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Resume.SendEmail"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Resume.AuditSetting"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Investigation"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Investigation.CreateOrEditOrDelete"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Investigation.FinshOrRestore"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Investigation.Audit"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Interview"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Interview.CreateOrEditOrDelete"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Evaluation"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Evaluation.CreateOrEditOrDelete"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.DailyStatistic"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.DailyStatistic.CreateOrEditOrDelete"),

                };
                foreach (var claim in claims)
                {
                    await _roleManager.AddClaimAsync(adminRole, claim);
                }
            }
            var adminUser = await _userManager.FindByNameAsync(UserDefaults.Admin);
            if (adminUser == null)
            {
                adminUser = new User()
                {
                    UserName = "Admin",
                    Email = "admin@junevan.com",
                    EmailConfirmed = true,
                    Name = "管理员",
                    Surname = "系统",
                    Confirmed = true,
                    PhoneNumber = "18689475927",
                    PhoneNumberConfirmed = true

                };
                await _userManager.CreateAsync(adminUser, "123qwe");

                await _userManager.AddToRoleAsync(adminUser, RoleDefaults.Administrators);
            }

        }
    }
}
