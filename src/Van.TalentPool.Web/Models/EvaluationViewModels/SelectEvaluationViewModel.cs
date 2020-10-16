using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Van.TalentPool.Web.Models.EvaluationViewModels
{
    public class SelectEvaluationViewModel
    {
        public Guid InvestigationId { get; set; }
        public string Name { get; set; }
        public List<SelectListItem> Evaluations { get; set; }
        [Required(ErrorMessage = "请选择技术评测题")]
        public Guid EvaluationId { get; set; }
    }
}
