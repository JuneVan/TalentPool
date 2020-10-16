using System;

namespace Van.TalentPool.Application.Evaluations
{
    public class QueryEvaluationInput : PaginationInput
    {
        public Guid? JobId { get; set; }
    }
}
