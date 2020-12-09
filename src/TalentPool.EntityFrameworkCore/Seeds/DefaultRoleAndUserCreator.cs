using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TalentPool.Roles;
using TalentPool.Users;

namespace TalentPool.EntityFrameworkCore.Seeds
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
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Authorization"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Authorization.Role"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Authorization.Role.CreateOrEditOrDelete"), 
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Authorization.Role.AssignPermission"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Authorization.User"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Authorization.User.CreateOrEditOrDelete"), 
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Authorization.User.AssignRole"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Configuration"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Configuration.SiteSetting"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Configuration.UserSetting"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Configuration.EmailSetting"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Configuration.Dictionary"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Configuration.Dictionary.CreateOrEditOrDelete"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Job"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Job.CreateOrEditOrDelete"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Resume"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Resume.CreateOrEditOrDelete"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Resume.AssignUser"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Resume.SendEmail"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Resume.AuditSetting"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Investigation"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Investigation.CreateOrEditOrDelete"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Investigation.FinshOrRestore"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Investigation.Audit"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Interview"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Interview.CreateOrEditOrDelete"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Evaluation"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.Evaluation.CreateOrEditOrDelete"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.DailyStatistic"),
                    new Claim(AppConstants.ClaimTypes.Permission,"Pages.DailyStatistic.CreateOrEditOrDelete"),

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
