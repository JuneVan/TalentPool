using System;

namespace Van.TalentPool.Application.Evaluations
{
    public class QueryQuestionInput : PaginationInput
    {
        public Guid EvaluationId { get; set; }
        public Guid SubjectId { get; set; }
    }
}
