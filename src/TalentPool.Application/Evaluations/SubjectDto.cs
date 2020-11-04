using System;

namespace TalentPool.Application.Evaluations
{
    public class SubjectDto
    {
        public Guid Id { get; set; }
        public string Keyword { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }
        public Guid EvaluationId { get; set; }
    }
}
