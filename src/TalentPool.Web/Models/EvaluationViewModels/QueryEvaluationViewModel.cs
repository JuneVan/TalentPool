using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using TalentPool.Application.Evaluations;
using TalentPool.Web.Models.CommonModels;

namespace TalentPool.Web.Models.EvaluationViewModels
{
    public class QueryEvaluationViewModel
    {
        public PaginationModel<EvaluationDto> Output { get; set; }
        public List<SelectListItem> Jobs { get; set; }
    }
}
