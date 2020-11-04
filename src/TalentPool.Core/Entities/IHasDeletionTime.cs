using System;

namespace TalentPool.Entities
{
    public interface IHasDeletionTime  
    {
        bool IsDeleted { get; set; }
        DateTime? DeletionTime { get; set; }
    }
}
