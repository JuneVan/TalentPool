using System;

namespace Une.TalentPool.Entities
{
    public interface IHasCreationTime
    {
        DateTime CreationTime { get; set; }
    }
}
