using System;

namespace Van.TalentPool.Entities
{
    public interface ICreationAudited : IHasCreationTime
    {
        Guid CreatorUserId { get; set; } 
    }
}
