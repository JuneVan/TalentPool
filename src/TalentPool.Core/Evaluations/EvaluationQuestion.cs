﻿using System;
using TalentPool.Entities;

namespace TalentPool.Evaluations
{
    public class EvaluationQuestion : Entity
    {
        public virtual Guid EvaluationId { get; set; }
        public virtual Guid SubjectId { get; set; }
        public virtual string Description { get; set; }
        public virtual string ReferenceAnswer { get; set; }
        public virtual int Order { get; set; }
    }
}
