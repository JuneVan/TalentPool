using System;
using System.Collections.Generic;
using Van.TalentPool.Entities;

namespace Van.TalentPool.Evaluations
{
    public class Evaluation : Entity, ICreationAudited, IModificationAudited, IDeletionAudited
    {
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }
        public virtual Guid JobId { get; set; }

        public virtual ICollection<EvaluationSubject> Subjects { get; set; }
        public virtual ICollection<EvaluationQuestion> Questions { get; set; }

        public virtual Guid CreatorUserId { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual Guid? LastModifierUserId { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual Guid? DeleterUserId { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
        public virtual bool IsDeleted { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
