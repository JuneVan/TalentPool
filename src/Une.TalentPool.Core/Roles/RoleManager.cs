using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Une.TalentPool.Roles
{
    public class RoleManager : RoleManager<Role>
    {
        private readonly IRoleStore _roleStore;
        private readonly ITokenProvider _tokenProvider;
        public RoleManager(IRoleStore store,
            IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            ILogger<RoleManager<Role>> logger,
            ITokenProvider  tokenProvider)
            : base(store,
            roleValidators,
            keyNormalizer, errors,
            logger)
        {
            _roleStore = store;
            _tokenProvider = tokenProvider;
        }
        protected override CancellationToken CancellationToken => _tokenProvider.Token;
        public async Task<IdentityResult> UpdatePermissionsAsync(Role role, List<string> permissions)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            if (permissions == null)
                throw new ArgumentNullException(nameof(permissions));

            // 移除旧的权限Claim数据
            var claims = await GetClaimsAsync(role);
            if (claims != null)
            {
                var permissionClaims = claims.Where(w => w.Type == AppConstansts.ClaimTypes.Permission).ToList();
                foreach (var permissionClaim in permissionClaims)
                {
                    await _roleStore.RemoveClaimAsync(role, permissionClaim, CancellationToken);
                }
            }
            // 增加权限Claim
            foreach (var permission in permissions)
            {
                await _roleStore.AddClaimAsync(role, new Claim(AppConstansts.ClaimTypes.Permission, permission), CancellationToken);
            }

            return await UpdateRoleAsync(role);

        }

        public async Task<List<string>> GetPermissionsAsync(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            return await _roleStore.GetPermissionClaimsAsync(role, CancellationToken);

        }
    }
}
