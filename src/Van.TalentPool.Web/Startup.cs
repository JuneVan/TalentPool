using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Van.TalentPool.Application.Roles;
using Van.TalentPool.Application.Users;
using Van.TalentPool.Configurations;
using Van.TalentPool.EntityFrameworkCore;
using Van.TalentPool.EntityFrameworkCore.Queriers;
using Van.TalentPool.EntityFrameworkCore.Seeds;
using Van.TalentPool.EntityFrameworkCore.Stores;
using Van.TalentPool.Infrastructure;
using Van.TalentPool.Infrastructure.Message.Email;
using Van.TalentPool.Infrastructure.Message.Sms;
using Van.TalentPool.Infrastructure.Notify;
using Van.TalentPool.Navigations;
using Van.TalentPool.Permissions;
using Van.TalentPool.Roles; 
using Van.TalentPool.Users;
using Van.TalentPool.Web.Auth;
using Van.TalentPool.Web.Profiles;

namespace Van.TalentPool.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<VanDbContext>(optionsAction =>
            {
                optionsAction.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.Configure<MvcOptions>(configureOptions =>
            {
                configureOptions.Filters.Add<NotifyFilter>();
            });
            // core
            services.AddTransient<IUserIdentifier, ClaimPrincipalUserIdentifier>();
            services.AddIdentity<User, Role>(opt =>
            {
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            })
                .AddUserStore<VanUserStore>()
                .AddUserManager<UserManager>()
                .AddRoleStore<VanRoleStore>()
                .AddRoleManager<RoleManager>()
                .AddSignInManager<SignInManager<User>>()
                .AddDefaultTokenProviders();
            services.AddTransient<IUserStore, VanUserStore>();
            services.AddTransient<IRoleStore, VanRoleStore>();
            services.AddTransient<IUserConfirmation<User>, UserActiveConfirmation>();
            services.AddTransient<ISettingValueStore, SettingValueStore>();
            services.AddTransient<SettingValueManager>();
            services.AddTransient<ConfigurationManager>();
            services.AddTransient<INavigationProvider, StandardNavigationProvider>();
            services.AddTransient<NavigationManager>();
            services.AddTransient<IPermissionProvider, StandardPermissionProvider>();
            services.AddTransient<PermissionManager>();
            // infrastructure
            services.AddScoped<INotifier, Notifier>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsSender, SmsSender>();

            // application-entityframework
            services.AddTransient<IRoleQuerier, RoleQuerier>();
            services.AddTransient<IUserQuerier, UserQuerier>();

            // automapper
            services.AddMappingProfiles();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //entityframework
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                new SeedHelper(serviceScope.ServiceProvider).SeedDb();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
