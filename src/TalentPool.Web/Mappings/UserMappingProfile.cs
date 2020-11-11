using AutoMapper;
using TalentPool.Users;
using TalentPool.Web.Models.UserViewModels;

namespace TalentPool.Web.Mappings
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
