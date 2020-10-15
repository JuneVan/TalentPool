using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Van.TalentPool.Application.Evaluations;
using Van.TalentPool.Web.Models.CommonModels;

namespace Van.TalentPool.Web.Models.EvaluationViewModels
{
    public class QueryEvaluationViewModel
    {
        public PaginationModel<EvaluationDto> Output { get; set; }
        public List<SelectListItem> Jobs { get; set; }
    }
}
