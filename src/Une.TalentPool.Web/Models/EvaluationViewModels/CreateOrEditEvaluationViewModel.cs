using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Une.TalentPool.Web.Models.EvaluationViewModels
{
    public class CreateOrEditEvaluationViewModel
    {
        public Guid? Id { get; set; }
        public List<SelectListItem> Jobs { get; set; }
        [Required(ErrorMessage = "请选择职位")]
        public Guid JobId { get; set; }
        public string Title { get; set; }
    }
}
