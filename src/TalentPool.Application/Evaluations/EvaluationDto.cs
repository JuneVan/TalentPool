using System;

namespace TalentPool.Application.Evaluations
{
    public class EvaluationDto : Dto
    {
        public string Title { get; set; }
        public Guid JobId { get; set; }
        public string JobName { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
