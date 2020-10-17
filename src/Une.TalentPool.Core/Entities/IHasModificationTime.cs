using System;

namespace Une.TalentPool.Entities
{
    public interface IHasModificationTime
    {
        DateTime? LastModificationTime { get; set; }
    }
}
