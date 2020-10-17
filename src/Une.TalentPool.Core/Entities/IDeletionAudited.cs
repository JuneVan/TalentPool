using System;

namespace Une.TalentPool.Entities
{
    public interface IDeletionAudited : IHasDeletionTime
    {
        Guid? DeleterUserId { get; set; } 
    }
}
