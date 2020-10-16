using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Van.TalentPool.Application.Interviews;
using Van.TalentPool.Web.Models.CommonModels;

namespace Van.TalentPool.Web.Models.InterviewViewModels
{
    public class QueryInterviewViewModel
    {
        public PaginationModel<InterviewDto> Output { get; set; }
        public List<SelectListItem> Jobs { get; set; }
        public List<SelectListItem> Users { get; set; }
    }
}
