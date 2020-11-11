using Microsoft.AspNetCore.Identity;
using System;
using TalentPool.Entities;

namespace TalentPool.Roles
{
    public class Role : IdentityRole<Guid>, ICreationAudited, IModificationAudited, IDeletionAudited
    {
        public Role()
        {

        }
        /// <summary>
        /// 显示名称
        /// </summary>
        public virtual string DisplayName { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 激活状态
        /// </summary>
        public virtual bool Active { get; set; }
        /// <summary>
        /// 受保护状态（不可删除）
        /// </summary>
        public virtual bool Protected { get; set; }


        public virtual Guid CreatorUserId { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual Guid? LastModifierUserId { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual Guid? DeleterUserId { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}
