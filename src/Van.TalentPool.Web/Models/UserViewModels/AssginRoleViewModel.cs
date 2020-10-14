using System;
using System.Collections.Generic;
using Van.TalentPool.Application.Roles;

namespace Van.TalentPool.Web.Models.UserViewModels
{
    public class AssginRoleViewModel
    {
        public Guid Id { get; set; } 
        public string UserName { get; set; }
        public List<RoleDto> Roles { get; set; }
        public List<string> SelectedRoles { get; set; }
    }
}
