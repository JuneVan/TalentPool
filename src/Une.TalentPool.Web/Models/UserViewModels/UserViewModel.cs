using System;

namespace Une.TalentPool.Web.Models.UserViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; } 
        public bool TwoFactorEnabled { get; set; } 
        public bool PhoneNumberConfirmed { get; set; } 
        public string PhoneNumber { get; set; } 
        public bool EmailConfirmed { get; set; } 
        public string Email { get; set; } 
        public string NormalizedUserName { get; set; } 
        public string UserName { get; set; } 
        public virtual string Name { get; set; } 
        public virtual string Surname { get; set; } 
        public virtual string Photo { get; set; } 
        public virtual bool Protected { get; set; } 
        public virtual bool Active { get; set; } 
        public virtual Guid CreatorUserId { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual Guid? LastModifierUserId { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual Guid? DeleterUserId { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}
