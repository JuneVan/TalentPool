using System;

namespace Une.TalentPool.Web.Models.RoleViewModels
{
    public class CreateOrEditRoleViewModel
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } 
        public string DisplayName { get; set; } 
        public bool Active { get; set; } 
    }
}
