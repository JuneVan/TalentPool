using System;
using TalentPool.Entities;

namespace TalentPool.Jobs
{
    /// <summary>
    /// 岗位
    /// </summary>
    public class Job : Entity, ICreationAudited, IModificationAudited, IDeletionAudited
    {
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 任职要求
        /// </summary>
        public virtual string Requirements { get; set; }
        /// <summary>
        /// 岗位职责
        /// </summary>
        public virtual string Description { get; set; } 
        /// <summary>
        /// 岗位职责关键词
        /// </summary>
        public virtual string Keywords { get; set; }
        /// <summary>
        /// 启用状态
        /// </summary>
        public virtual bool Enable { get; set; }
        /// <summary>
        /// 薪水范围
        /// </summary>
        public virtual string SalaryRange { get; set; }
        /// <summary>
        /// 性别范围
        /// </summary>
        public virtual string GenderRange { get; set; }
        /// <summary>
        /// 年龄范围
        /// </summary>
        public virtual string AgeRange { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }


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
