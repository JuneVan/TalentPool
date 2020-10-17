using System;

namespace Une.TalentPool.Entities
{
    public interface ICreationAudited : IHasCreationTime
    {
        Guid CreatorUserId { get; set; } 
    }
}
