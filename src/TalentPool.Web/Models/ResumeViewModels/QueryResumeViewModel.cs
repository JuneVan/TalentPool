using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using TalentPool.Application.Resumes;
using TalentPool.Web.Models.CommonModels;

namespace TalentPool.Web.Models.ResumeViewModels
{
    public class QueryResumeViewModel
    {
        public PaginationModel<ResumeDto> Output { get; set; }
        public List<SelectListItem> Jobs { get; set; }
        public List<SelectListItem> Users { get; set; } 
    }
}
