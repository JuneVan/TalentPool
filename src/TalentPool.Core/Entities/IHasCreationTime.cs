using System;

namespace TalentPool.Entities
{
    public interface IHasCreationTime
    {
        DateTime CreationTime { get; set; }
    }
}
