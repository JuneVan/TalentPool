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
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Authorization.Role.Create"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Authorization.Role.Edit"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Authorization.Role.Delete"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Authorization.Role.AssignPermission"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Authorization.User"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Authorization.User.Create"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Authorization.User.Edit"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Authorization.User.Delete"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Authorization.User.AssignRole"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Configuration"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Configuration.SiteSetting"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Configuration.UserSetting"),
                    new Claim(AppConstansts.ClaimTypes.Permission,"Pages.Configuration.EmailSetting"),

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
