using System.Collections.Generic;

namespace TalentPool.Roles
{
    public interface IPermissionProvider
    {
        IEnumerable<PermissionDefinition> Definitions();
    }
}
