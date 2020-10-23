using AutoMapper;
using Une.TalentPool.Configurations;
using Une.TalentPool.Web.Models.SettingViewModels;

namespace Une.TalentPool.Web.Profiles
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
            CreateMap<UserCustomSetting, UserCustomSettingViewModel>();
            CreateMap<UserCustomSettingViewModel, UserCustomSetting>();
            CreateMap<ResumeSetting, ResumeSettingViewModel>();
            CreateMap<ResumeSettingViewModel, ResumeSetting>();
        }
    }
}
