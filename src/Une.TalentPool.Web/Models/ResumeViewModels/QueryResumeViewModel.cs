using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Une.TalentPool.Application.Resumes;
using Une.TalentPool.Web.Models.CommonModels;

namespace Une.TalentPool.Web.Models.ResumeViewModels
{
    public class QueryResumeViewModel
    {
        public PaginationModel<ResumeDto> Output { get; set; }
        public List<SelectListItem> Jobs { get; set; }
        public List<SelectListItem> Users { get; set; } 
    }
}
