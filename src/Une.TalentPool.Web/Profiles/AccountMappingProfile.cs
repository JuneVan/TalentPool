using AutoMapper;
using Une.TalentPool.Users;
using Une.TalentPool.Web.Models.AccountViewModels;

namespace Une.TalentPool.Web.Profiles
{
    public class AccountMappingProfile:Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<User, UpdateProfileViewModel>();
            CreateMap<UpdateProfileViewModel, User>()
                 .ForMember(m => m.Photo, opts => opts.Ignore());
        }
    }
}
