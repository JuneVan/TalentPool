using AutoMapper;
using Van.TalentPool.Configurations;
using Van.TalentPool.Web.Models.SettingViewModels;

namespace Van.TalentPool.Web.Profiles
{
    public class SettingMappingProfile : Profile
    {
        public SettingMappingProfile()
        {
            CreateMap<SiteSetting, SiteSettingViewModel>();
            CreateMap<SiteSettingViewModel, SiteSetting>();
            CreateMap<UserSetting, UserSettingViewModel>();
            CreateMap<UserSettingViewModel, UserSetting>();
            CreateMap<EmailSetting, EmailSettingViewModel>();
            CreateMap<EmailSettingViewModel, EmailSetting>();
        }
    }
}
