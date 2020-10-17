using System;

namespace Une.TalentPool.Application.Evaluations
{
    public class QuerySubjectInput : PaginationInput
    {
        public Guid EvaluationId { get; set; }
    }
}
