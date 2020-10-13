using System;
using System.Collections.Generic;
using Van.TalentPool.Permissions;

namespace Van.TalentPool.Web.Models.RoleViewModels
{
    public class AssignPermissionViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public List<PermissionDefinition> Permissions { get; set; }
    }
}
