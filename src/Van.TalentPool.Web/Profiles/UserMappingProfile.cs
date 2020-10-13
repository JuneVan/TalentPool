using AutoMapper;
using Van.TalentPool.Users;
using Van.TalentPool.Web.Models.UserViewModels;

namespace Van.TalentPool.Web.Profiles
{
    public class UserMappingProfile:Profile
    {
        public UserMappingProfile()
        {
            CreateMap<CreateOrEditUserViewModel, User>()
              .ForMember(m => m.Photo, opts => opts.Ignore());
            CreateMap<User, CreateOrEditUserViewModel>();
            CreateMap<User, AssginRoleViewModel>();
            CreateMap<User, UserViewModel>();
        }
    }
}
