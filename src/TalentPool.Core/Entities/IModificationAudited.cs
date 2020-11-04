using System;

namespace TalentPool.Entities
{
    public interface IModificationAudited : IHasModificationTime
    {
        Guid? LastModifierUserId { get; set; }   
    }
}
