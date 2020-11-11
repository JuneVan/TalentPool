using System;
using System.Collections.Generic;
using System.Text;

namespace TalentPool.Permissions
{
    public interface IPermissionProvider
    {
        IEnumerable<PermissionDefinition> Definitions();
    }
}
