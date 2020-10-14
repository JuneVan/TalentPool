using System;

namespace Van.TalentPool
{
    public interface IUserIdentifier
    {
        Guid? UserId { get; }
    }
}
