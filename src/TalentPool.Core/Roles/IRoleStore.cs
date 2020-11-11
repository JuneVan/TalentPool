using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TalentPool.Roles
{
    public interface IRoleStore :
         IQueryableRoleStore<Role>,
        IRoleClaimStore<Role>
    {
        Task<List<string>> GetPermissionClaimsAsync(Role role, CancellationToken cancellationToken = default);
    }
}
