using System;
using System.Collections.Generic;
using System.Text;

namespace Une.TalentPool.Permissions
{
    public interface IPermissionProvider
    {
        IEnumerable<PermissionDefinition> Definitions();
    }
}
