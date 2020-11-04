using System;

namespace TalentPool
{
    public interface IUserIdentifier
    {
        Guid? UserId { get; }
    }
}
