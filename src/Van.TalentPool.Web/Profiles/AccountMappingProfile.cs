using AutoMapper;
using Van.TalentPool.Users;
using Van.TalentPool.Web.Models.AccountViewModels;

namespace Van.TalentPool.Web.Profiles
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
