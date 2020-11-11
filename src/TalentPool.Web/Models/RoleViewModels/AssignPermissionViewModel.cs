using System;
using System.Collections.Generic;
using TalentPool.Permissions;

namespace TalentPool.Web.Models.RoleViewModels
{
    public class AssignPermissionViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public List<PermissionDefinition> Permissions { get; set; }
    }
}
