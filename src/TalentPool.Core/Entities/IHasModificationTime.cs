using System;

namespace TalentPool.Entities
{
    public interface IHasModificationTime
    {
        DateTime? LastModificationTime { get; set; }
    }
}
