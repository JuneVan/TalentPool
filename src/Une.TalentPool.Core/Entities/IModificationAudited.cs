using System;

namespace Une.TalentPool.Entities
{
    public interface IModificationAudited : IHasModificationTime
    {
        Guid? LastModifierUserId { get; set; }   
    }
}
