using System;
using Une.TalentPool.Entities;

namespace Une.TalentPool.Resumes
{
    public class ResumeAuditRecord : Entity, ICreationAudited
    {
        /// <summary>
        /// 简历编号
        /// </summary>
        public Guid ResumeId { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否审核通过
        /// </summary>
        public bool Passed { get; set; } 
        public virtual Guid CreatorUserId { get; set; }
        public virtual DateTime CreationTime { get; set; }
    }
}
