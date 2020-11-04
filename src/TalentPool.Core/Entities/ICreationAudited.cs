using System;

namespace TalentPool.Entities
{
    public interface ICreationAudited : IHasCreationTime
    {
        Guid CreatorUserId { get; set; } 
    }
}
