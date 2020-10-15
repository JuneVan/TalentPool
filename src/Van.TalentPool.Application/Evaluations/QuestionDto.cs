using System;

namespace Van.TalentPool.Application.Evaluations
{
    public class QuestionDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }
}
