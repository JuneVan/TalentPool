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
            services.AddAutoMapper(typeof(SettingMappingProfile));
            return services;
        }
    }
}
