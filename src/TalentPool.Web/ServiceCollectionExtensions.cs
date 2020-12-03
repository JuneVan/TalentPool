using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TalentPool.Application.DailyStatistics;
using TalentPool.Application.Dictionaries;
using TalentPool.Application.Evaluations;
using TalentPool.Application.Interviews;
using TalentPool.Application.Investigations;
using TalentPool.Application.Jobs;
using TalentPool.Application.Resumes;
using TalentPool.Application.Roles;
using TalentPool.Application.Users;
using TalentPool.Configurations;
using TalentPool.DailyStatistics;
using TalentPool.Dictionaries;
using TalentPool.EntityFrameworkCore;
using TalentPool.EntityFrameworkCore.Queriers;
using TalentPool.EntityFrameworkCore.Stores;
using TalentPool.Evaluations;
using TalentPool.Infrastructure;
using TalentPool.Infrastructure.Authorize;
using TalentPool.Infrastructure.Message.Email;
using TalentPool.Infrastructure.Message.Sms;
using TalentPool.Infrastructure.Notify;
using TalentPool.Interviews;
using TalentPool.Investigations;
using TalentPool.Jobs;
using TalentPool.Navigations;
using TalentPool.Permissions;
using TalentPool.Resumes;
using TalentPool.Roles;
using TalentPool.Users;
using TalentPool.Web.Auth;
using TalentPool.Web.Mappings;

namespace TalentPool.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTalentPoolWeb(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<MvcOptions>(cfg =>
            {
                cfg.Filters.Add<NotifyFilter>(); 
                cfg.Filters.Add<AuthorizationFilter>();
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
            services.AddMediatR(typeof(ResumeDeletedEvent).Assembly);
            services.AddTransient<ResumeManager>();
            services.AddTransient<ResumeAuditSettingManager>();
            services.AddTransient<InvestigationManager>();
            services.AddTransient<IInvestigaionValidator,ResumeIdValidator>();
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
