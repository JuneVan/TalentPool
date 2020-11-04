using AutoMapper;
using TalentPool.Users;
using TalentPool.Web.Models.AccountViewModels;

namespace TalentPool.Web.Mappings
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
