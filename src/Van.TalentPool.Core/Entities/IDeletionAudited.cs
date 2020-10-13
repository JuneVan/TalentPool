using System;

namespace Van.TalentPool.Entities
{
    public interface IDeletionAudited : IHasDeletionTime
    {
        Guid? DeleterUserId { get; set; } 
    }
}
