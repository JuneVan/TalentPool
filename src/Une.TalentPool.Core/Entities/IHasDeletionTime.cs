using System;

namespace Une.TalentPool.Entities
{
    public interface IHasDeletionTime  
    {
        bool IsDeleted { get; set; }
        DateTime? DeletionTime { get; set; }
    }
}
