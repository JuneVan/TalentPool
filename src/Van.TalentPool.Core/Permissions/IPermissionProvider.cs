using System;
using System.Collections.Generic;
using System.Text;

namespace Van.TalentPool.Permissions
{
    public interface IPermissionProvider
    {
        IEnumerable<PermissionDefinition> Definitions();
    }
}
