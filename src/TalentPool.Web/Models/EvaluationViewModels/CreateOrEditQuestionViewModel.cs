using System;
using System.ComponentModel.DataAnnotations;

namespace TalentPool.Web.Models.EvaluationViewModels
{
    public class CreateOrEditQuestionViewModel
    {
        [Required(ErrorMessage = "请填写问题描述信息")]
        public string Description { get; set; }
        public string ReferenceAnswer { get; set; }
        public int Order { get; set; }
        public Guid SubjectId { get; set; }
        public Guid EvaluationId { get; set; }
        public Guid? Id { get; set; }
    }
}
