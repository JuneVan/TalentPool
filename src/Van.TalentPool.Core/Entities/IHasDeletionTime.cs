using System;

namespace Van.TalentPool.Entities
{
    public interface IHasDeletionTime  
    {
        bool IsDeleted { get; set; }
        DateTime? DeletionTime { get; set; }
    }
}
