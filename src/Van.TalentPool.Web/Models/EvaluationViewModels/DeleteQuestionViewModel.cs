using System;

namespace Van.TalentPool.Web.Models.EvaluationViewModels
{
    public class DeleteQuestionViewModel
    {
        public Guid Id { get; set; }
        public Guid SubjectId { get; set; }
        public Guid EvaluationId { get; set; }
        public string Description { get; set; }
    }
}
