using System;
using Une.TalentPool.Entities;

namespace Une.TalentPool.Interviews
{
    public class Interview : Entity, ICreationAudited, IModificationAudited, IDeletionAudited
    {
        /// <summary>
        /// 简历
        /// </summary>
        public virtual Guid ResumeId { get; set; } 
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; } 
        /// <summary>
        /// 姓名
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public virtual DateTime AppointmentTime { get; set; }
        /// <summary>
        /// 到访时间
        /// </summary>
        public virtual DateTime? VisitedTime { get; set; }
        /// <summary>
        /// 预约状态
        /// </summary>
        public virtual InterviewStatus Status { get; set; }

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
