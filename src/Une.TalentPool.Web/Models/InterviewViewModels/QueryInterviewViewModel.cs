using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Une.TalentPool.Application.Interviews;
using Une.TalentPool.Web.Models.CommonModels;

namespace Une.TalentPool.Web.Models.InterviewViewModels
{
    public class QueryInterviewViewModel
    {
        public PaginationModel<InterviewDto> Output { get; set; }
        public List<SelectListItem> Jobs { get; set; }
        public List<SelectListItem> Users { get; set; }
    }
}
