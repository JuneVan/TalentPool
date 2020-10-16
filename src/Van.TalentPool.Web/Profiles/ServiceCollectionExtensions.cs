using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Van.TalentPool.Web.Profiles
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMappingProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AccountMappingProfile));
            services.AddAutoMapper(typeof(RoleMappingProfile));
            services.AddAutoMapper(typeof(UserMappingProfile));
            services.AddAutoMapper(typeof(DictionaryMappingProfile));
            services.AddAutoMapper(typeof(SettingMappingProfile));
            services.AddAutoMapper(typeof(JobMappingProfile));
            services.AddAutoMapper(typeof(ResumeMappingProfile));
            services.AddAutoMapper(typeof(InvestigationMappingProfile));
            services.AddAutoMapper(typeof(InterviewMappingProfile));
            services.AddAutoMapper(typeof(EvaluationMappingProfile));
            services.AddAutoMapper(typeof(DailyStatisticMappingProfile));
            return services;
        }
    }
}
