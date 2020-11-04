using Microsoft.AspNetCore.Identity;
using System;
using TalentPool.Entities;

namespace TalentPool.Users
{
    public class User : IdentityUser<Guid>, ICreationAudited, IModificationAudited, IDeletionAudited
    {
        public User()
        {

        } 
        /// <summary>
        /// 名
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 姓
        /// </summary>
        public virtual string Surname { get; set; }
        public string FullName => Surname + Name;
        /// <summary>
        /// 个人头像
        /// </summary>
        public virtual string Photo { get; set; }
        /// <summary>
        /// 受保护状态（不可删除）
        /// </summary>
        public virtual bool Protected { get; set; } 

        public virtual bool Confirmed { get; set; }

        public virtual Guid CreatorUserId { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual Guid? LastModifierUserId { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual Guid? DeleterUserId { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}
