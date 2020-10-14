using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Van.TalentPool.Application.Resumes;
using Van.TalentPool.Web.Models.CommonModels;

namespace Van.TalentPool.Web.Models.ResumeViewModels
{
    public class QueryResumeViewModel
    {
        public PaginationModel<ResumeDto> Pagination { get; set; }
        public List<SelectListItem> Jobs { get; set; }
        public List<SelectListItem> Users { get; set; } 
    }
}
