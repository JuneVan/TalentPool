using Van.TalentPool.Entities;

namespace Van.TalentPool.Evaluations
{
    public class EvaluationQuestion : Entity
    {
        public virtual string Description { get; set; }
        public virtual string ReferenceAnswer { get; set; }
    }
}
