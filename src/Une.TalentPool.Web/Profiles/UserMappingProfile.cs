using AutoMapper;
using Une.TalentPool.Users;
using Une.TalentPool.Web.Models.UserViewModels;

namespace Une.TalentPool.Web.Profiles
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
