using System;
using System.Collections.Generic;

namespace Une.TalentPool.Web.Models.InvestigationViewModels
{
    public class EvaluateResultModel
    {
        public Guid InvestigationId { get; set; }
        public List<Question> Questions { get; set; }
    }
    public class Question
    {
        public string Keyword { get; set; }
        public string Description { get; set; }
        public float Score { get; set; }
    }
}
