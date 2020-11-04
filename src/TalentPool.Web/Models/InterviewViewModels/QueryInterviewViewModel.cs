using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using TalentPool.Application.Interviews;
using TalentPool.Web.Models.CommonModels;

namespace TalentPool.Web.Models.InterviewViewModels
{
    public class QueryInterviewViewModel
    {
        public PaginationModel<InterviewDto> Output { get; set; }
        public List<SelectListItem> Jobs { get; set; }
        public List<SelectListItem> Users { get; set; }
    }
}
