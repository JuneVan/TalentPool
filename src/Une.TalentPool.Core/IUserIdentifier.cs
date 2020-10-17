using System;

namespace Une.TalentPool
{
    public interface IUserIdentifier
    {
        Guid? UserId { get; }
    }
}
