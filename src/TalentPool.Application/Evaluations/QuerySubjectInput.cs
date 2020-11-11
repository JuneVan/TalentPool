using System;

namespace TalentPool.Application.Evaluations
{
    public class QuerySubjectInput : PaginationInput
    {
        public Guid EvaluationId { get; set; }
    }
}
