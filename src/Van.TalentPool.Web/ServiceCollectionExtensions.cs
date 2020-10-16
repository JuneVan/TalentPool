using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Van.TalentPool.Application.DailyStatistics;
using Van.TalentPool.Application.Dictionaries;
using Van.TalentPool.Application.Evaluations;
using Van.TalentPool.Application.Interviews;
using Van.TalentPool.Application.Investigations;
using Van.TalentPool.Application.Jobs;
using Van.TalentPool.Application.Resumes;
using Van.TalentPool.Application.Roles;
using Van.TalentPool.Application.Users;
using Van.TalentPool.Configurations;
using Van.TalentPool.DailyStatistics;
using Van.TalentPool.Dictionaries;
using Van.TalentPool.EntityFrameworkCore.Queriers;
using Van.TalentPool.EntityFrameworkCore.Stores;
using Van.TalentPool.Evaluations;
using Van.TalentPool.Infrastructure;
using Van.TalentPool.Infrastructure.Exceptions;
using Van.TalentPool.Infrastructure.Message.Email;
using Van.TalentPool.Infrastructure.Message.Sms;
using Van.TalentPool.Infrastructure.Notify;
using Van.TalentPool.Interviews;
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
    public static  class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTalentPoolWeb(this IServiceCollection services)
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
            .AddUserStore<VanUserStore>()
            .AddUserManager<UserManager>()
            .AddRoleStore<VanRoleStore>()
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
            services.AddScoped<INotifier, Notifier>();
            services.AddTransient<INotifySerializer, NotifySerializer>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsSender, SmsSender>();
            services.AddMemoryCache();

            // entityframework 
            services.AddTransient<IUserStore, VanUserStore>();
            services.AddTransient<IRoleStore, VanRoleStore>();
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


            return services;
        }
    }
}
