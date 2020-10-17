using AutoMapper;
using Une.TalentPool.Roles;
using Une.TalentPool.Web.Models.RoleViewModels;

namespace Une.TalentPool.Web.Profiles
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
