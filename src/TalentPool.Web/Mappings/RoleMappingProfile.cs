using AutoMapper;
using TalentPool.Roles;
using TalentPool.Web.Models.RoleViewModels;

namespace TalentPool.Web.Mappings
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
