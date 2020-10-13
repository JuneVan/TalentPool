using AutoMapper;
using Van.TalentPool.Roles;
using Van.TalentPool.Web.Models.RoleViewModels;

namespace Van.TalentPool.Web.Profiles
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<Role, CreateOrEditRoleViewModel>();
            CreateMap<CreateOrEditRoleViewModel, Role>();
            CreateMap<Role, DeleteRoleViewModel>();
            CreateMap<Role, AssignPermissionViewModel>();
        }
    }
}
