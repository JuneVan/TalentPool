using System;

namespace Van.TalentPool.Application.Evaluations
{
    public class QuerySubjectInput : PaginationInput
    {
        public Guid EvaluationId { get; set; }
    }
}
