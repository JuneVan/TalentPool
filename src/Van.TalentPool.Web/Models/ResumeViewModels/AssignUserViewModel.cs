using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Van.TalentPool.Web.Models.ResumeViewModels
{
    public class AssignUserViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OwnerUserId { get; set; }  
        public string OwnerUserName { get; set; }
        public List<SelectListItem> Users { get; set; }
    } 
}
