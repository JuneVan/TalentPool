using System;
using System.Collections.Generic;
using Une.TalentPool.Entities;

namespace Une.TalentPool.Evaluations
{
    public class EvaluationSubject : Entity
    {
        public virtual Guid EvaluationId { get; set; }
        public virtual string Keyword { get; set; }
        public virtual string Description { get; set; }
        public virtual int Weight { get; set; }
        public virtual ICollection<EvaluationQuestion> Questions { get; set; }
    }
}
