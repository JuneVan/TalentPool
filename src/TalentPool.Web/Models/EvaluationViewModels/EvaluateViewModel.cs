using System;

namespace TalentPool.Web.Models.EvaluationViewModels
{
    public class EvaluateViewModel
    {
        public EvaluationViewModel Evaluation { get; set; }
        public Guid InvestigationId { get; set; }
        public string Name { get; set; }
    }
}
