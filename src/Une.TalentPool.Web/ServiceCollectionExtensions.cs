using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Une.TalentPool.Application.DailyStatistics;
using Une.TalentPool.Application.Dictionaries;
using Une.TalentPool.Application.Evaluations;
using Une.TalentPool.Application.Interviews;
using Une.TalentPool.Application.Investigations;
using Une.TalentPool.Application.Jobs;
using Une.TalentPool.Application.Resumes;
using Une.TalentPool.Application.Roles;
using Une.TalentPool.Application.Users;
using Une.TalentPool.Configurations;
using Une.TalentPool.DailyStatistics;
using Une.TalentPool.Dictionaries;
using Une.TalentPool.EntityFrameworkCore;
using Une.TalentPool.EntityFrameworkCore.Queriers;
using Une.TalentPool.EntityFrameworkCore.Stores;
using Une.TalentPool.Evaluations;
using Une.TalentPool.Infrastructure;
using Une.TalentPool.Infrastructure.Exceptions;
using Une.TalentPool.Infrastructure.Message.Email;
using Une.TalentPool.Infrastructure.Message.Sms;
using Une.TalentPool.Infrastructure.Notify;
using Une.TalentPool.Interviews;
using Une.TalentPool.Investigations;
using Une.TalentPool.Jobs;
using Une.TalentPool.Navigations;
using Une.TalentPool.Permissions;
using Une.TalentPool.Resumes;
using Une.TalentPool.Roles;
using Une.TalentPool.Users;
using Une.TalentPool.Web.Auth;
using Une.TalentPool.Web.Profiles;

namespace Une.TalentPool.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTalentPoolWeb(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<MvcOptions>(cfg =>
            {
                cfg.Filters.Add<NotifyFilter>();
                cfg.Filters.Add<GlobalExceptionFilter>();
            });
            // core
            services.AddTransient<IUserIdentifier, ClaimPrincipalUserIdentifier>();
            services.AddIdentity<User, Role>(opt =>
            {
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddUserStore<UserStore>()
            .AddUserManager<UserManager>()
            .AddRoleStore<RoleStore>()
            .AddRoleManager<RoleManager>()
            .AddDefaultTokenProviders();
            services.AddTransient<IUserConfirmation<User>, UserActiveConfirmation>();
            services.AddTransient<SettingValueManager>();
            services.AddTransient<ConfigurationManager>();
            services.AddTransient<DictionaryManager>();
            services.Configure<DictionaryOptions>(cfg =>
            {
                cfg.Injects = new[]
                {
                    new Dictionary(){
                        Name=ResumeDefaults.PlatformType,
                        DisplayName="简历来源网站"
                    }
                };
            });
            services.AddTransient<INavigationProvider, StandardNavigationProvider>();
            services.AddTransient<NavigationManager>();
            services.AddTransient<IPermissionProvider, StandardPermissionProvider>();
            services.AddTransient<PermissionManager>();
            services.AddTransient<JobManager>();
            services.AddTransient<IResumeValidator, PhoneNumberValidator>();
            services.AddTransient<IResumeValidator, PlatformValidator>();
            services.AddTransient<IResumeComparer, ResumeComparer>();
            services.AddTransient<ResumeManager>();
            services.AddTransient<ResumeAuditSettingManager>();
            services.AddTransient<InvestigationManager>();
            services.AddTransient<InterviewManager>();
            services.AddTransient<EvaluationManager>();
            services.AddTransient<DailyStatisticManager>();

            // infrastructure
            services.AddHttpContextAccessor();
            services.AddScoped<INotifier, Notifier>();
            services.AddTransient<INotifySerializer, NotifySerializer>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsSender, SmsSender>();
            services.AddTransient<ITokenProvider, HttpContextCancellationTokenProvider>();
            services.AddMemoryCache();

            // entityframework 
            services.AddDbContext<TalentDbContext>(optionsAction =>
            {
                optionsAction.UseMySql(configuration.GetConnectionString("DefaultConnection"));
            }, optionsLifetime: ServiceLifetime.Transient);
            services.AddTransient<IUserStore, UserStore>();
            services.AddTransient<IRoleStore, RoleStore>();
            services.AddTransient<ISettingValueStore, SettingValueStore>();
            services.AddTransient<IDictionaryStore, DictionaryStore>();
            services.AddTransient<IJobStore, JobStore>();
            services.AddTransient<IResumeStore, ResumeStore>();
            services.AddTransient<IResumeAuditSettingStore, ResumeAuditSettingStore>();
            services.AddTransient<IInvestigationStore, InvestigationStore>();
            services.AddTransient<IInterviewStore, InterviewStore>();
            services.AddTransient<IEvaluationStore, EvaluationStore>();
            services.AddTransient<IDailyStatisticStore, DailyStatisticStore>();


            services.AddTransient<IRoleQuerier, RoleQuerier>();
            services.AddTransient<IUserQuerier, UserQuerier>();
            services.AddTransient<IDictionaryQuerier, DictionaryQuerier>();
            services.AddTransient<IJobQuerier, JobQuerier>();
            services.AddTransient<IResumeQuerier, ResumeQuerier>();
            services.AddTransient<IInvestigationQuerier, InvestigationQuerier>();
            services.AddTransient<IInterviewQuerier, InterviewQuerier>();
            services.AddTransient<IEvaluationQuerier, EvaluationQuerier>();
            services.AddTransient<IDailyStatisticQuerier, DailyStatisticQuerier>();

            // web - automapper
            services.AddMappingProfiles();

            ////dapper
            //services.AddTransient<IConnectionProvider, MysqlConnectionProvider>();
            //services.AddTransient<IResumeQuerier, ResumeQuerier>();
            //services.Configure<DapperOptions>(cfg =>
            //{
            //    cfg.ConnectionString = configuration.GetConnectionString("DefaultConnection");
            //});


            return services;
        }
    }
}
