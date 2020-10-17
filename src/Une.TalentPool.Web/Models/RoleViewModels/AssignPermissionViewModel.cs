using System;
using System.Collections.Generic;
using Une.TalentPool.Permissions;

namespace Une.TalentPool.Web.Models.RoleViewModels
{
    public class AssignPermissionViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public List<PermissionDefinition> Permissions { get; set; }
    }
}
