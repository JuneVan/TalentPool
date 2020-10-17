using System;

namespace Une.TalentPool.Application.Evaluations
{
    public class QueryEvaluationInput : PaginationInput
    {
        public Guid? JobId { get; set; }
    }
}
