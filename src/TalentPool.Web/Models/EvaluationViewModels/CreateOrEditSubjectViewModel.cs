using System;
using System.ComponentModel.DataAnnotations;

namespace TalentPool.Web.Models.EvaluationViewModels
{
    public class CreateOrEditSubjectViewModel
    {
        [Required(ErrorMessage = "请填写技术点关键项")]
        public string Keyword { get; set; }

        [Required(ErrorMessage = "请填写技术点描述信息")]
        public string Description { get; set; }
        public int Weight { get; set; }
        public Guid EvaluationId { get; set; }
        public Guid? Id { get; set; }
    }
}
