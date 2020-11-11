using System;
using TalentPool.Entities;

namespace TalentPool.Resumes
{
    public class ResumeCompare : Entity
    {
        /// <summary>
        /// 相似度
        /// </summary>
        public virtual decimal Similarity { get; set; }
        /// <summary>
        /// 当前简历
        /// </summary>
        public virtual Guid ResumeId { get; set; }
        /// <summary>
        /// 关联相似简历
        /// </summary>
        public virtual Guid RelationResumeId { get; set; }
        /// <summary>
        /// 关联相似简历姓名
        /// </summary>
        public virtual string RelationResumeName { get; set; }

    }
}
