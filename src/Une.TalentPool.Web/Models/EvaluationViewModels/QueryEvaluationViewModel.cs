using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Une.TalentPool.Application.Evaluations;
using Une.TalentPool.Web.Models.CommonModels;

namespace Une.TalentPool.Web.Models.EvaluationViewModels
{
    public class QueryEvaluationViewModel
    {
        public PaginationModel<EvaluationDto> Output { get; set; }
        public List<SelectListItem> Jobs { get; set; }
    }
}
