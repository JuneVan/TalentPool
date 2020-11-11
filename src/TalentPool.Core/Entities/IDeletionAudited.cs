using System;

namespace TalentPool.Entities
{
    public interface IDeletionAudited : IHasDeletionTime
    {
        Guid? DeleterUserId { get; set; } 
    }
}
