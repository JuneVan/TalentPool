using System;

namespace Van.TalentPool.Entities
{
    public interface IHasCreationTime
    {
        DateTime CreationTime { get; set; }
    }
}
