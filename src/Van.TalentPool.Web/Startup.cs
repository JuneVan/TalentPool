using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Van.TalentPool.Application.Dictionaries;
using Van.TalentPool.Application.Investigations;
using Van.TalentPool.Application.Jobs;
using Van.TalentPool.Application.Resumes;
using Van.TalentPool.Application.Roles;
using Van.TalentPool.Application.Users;
using Van.TalentPool.Configurations;
using Van.TalentPool.Dictionaries;
using Van.TalentPool.EntityFrameworkCore;
using Van.TalentPool.EntityFrameworkCore.Queriers;
using Van.TalentPool.EntityFrameworkCore.Seeds;
using Van.TalentPool.EntityFrameworkCore.Stores;
using Van.TalentPool.Infrastructure;
using Van.TalentPool.Infrastructure.Message.Email;
using Van.TalentPool.Infrastructure.Message.Sms;
using Van.TalentPool.Infrastructure.Notify;
using Van.TalentPool.Investigations;
using Van.TalentPool.Jobs;
using Van.TalentPool.Navigations;
using Van.TalentPool.Permissions;
using Van.TalentPool.Resumes;
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
            services.AddTransient<IDictionaryStore, DictionaryStore>();
            services.AddTransient<DictionaryManager>();
            services.Configure<DictionaryOptions>(cfg =>
            {
                cfg.Injects = new[]
                {
                    new Dictionary(){
                        Name=ResumeDefaults.PlatformType,
                        DisplayName="ºÚ¿˙¿¥‘¥Õ¯’æ"
                    }
                };
            });
            services.AddTransient<INavigationProvider, StandardNavigationProvider>();
            services.AddTransient<NavigationManager>();
            services.AddTransient<IPermissionProvider, StandardPermissionProvider>();
            services.AddTransient<PermissionManager>();
            services.AddTransient<IJobStore, JobStore>();
            services.AddTransient<JobManager>();
            services.AddTransient<IResumeStore, ResumeStore>();
            services.AddTransient<IResumeValidator, PhoneNumberValidator>();
            services.AddTransient<IResumeValidator, PlatformValidator>();
            services.AddTransient<IResumeComparer, ResumeComparer>();
            services.AddTransient<ResumeManager>();
            services.AddTransient<IResumeAuditSettingStore, ResumeAuditSettingStore>();
            services.AddTransient<ResumeAuditSettingManager>();
            services.AddTransient<IInvestigationStore, InvestigationStore>();
            services.AddTransient<InvestigationManager>();

            // infrastructure
            services.AddScoped<INotifier, Notifier>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsSender, SmsSender>();
            services.AddMemoryCache();

            // application-entityframework
            services.AddTransient<IRoleQuerier, RoleQuerier>();
            services.AddTransient<IUserQuerier, UserQuerier>();
            services.AddTransient<IDictionaryQuerier, DictionaryQuerier>();
            services.AddTransient<IJobQuerier, JobQuerier>();
            services.AddTransient<IResumeQuerier, ResumeQuerier>();
            services.AddTransient<IInvestigationQuerier, InvestigationQuerier>();

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
