using System;

namespace Van.TalentPool.Entities
{
    public interface IModificationAudited : IHasModificationTime
    {
        Guid? LastModifierUserId { get; set; }   
    }
}
