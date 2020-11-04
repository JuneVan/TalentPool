using System;

namespace TalentPool.Web.Models.EvaluationViewModels
{
    public class DeleteSubjectViewModel
    {
        public Guid Id { get; set; }
        public Guid EvaluationId { get; set; }
        public string Keyword { get; set; }
    }
}
