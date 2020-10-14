using System;

namespace Van.TalentPool.Entities
{
    public interface IHasModificationTime
    {
        DateTime? LastModificationTime { get; set; }
    }
}
