using System;
using Van.TalentPool.Entities;

namespace Van.TalentPool.Resumes
{
    public class ResumeCompare:Entity
    {
        /// <summary>
        /// 相似度
        /// </summary>
        public virtual int Similarity { get; set; }
        /// <summary>
        /// 当前简历
        /// </summary>
        public virtual Guid ResumeId { get; set; }
        /// <summary>
        /// 关联相似简历
        /// </summary>
        public virtual Guid RelationResumeId { get; set; }
    }
}
