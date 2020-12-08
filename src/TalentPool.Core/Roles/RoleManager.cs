using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace TalentPool.Roles
{
    public class RoleManager : RoleManager<Role>
    { 
        public RoleManager(IRoleStore roleStore,
            IServiceProvider serviceProvider,
            IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            ILogger<RoleManager<Role>> logger,
            ISignal  signal)
            : base(roleStore,
            roleValidators,
            keyNormalizer, errors,
            logger)
        {
            RoleStore = roleStore;
            Signal = signal;
            PermissionProviders = serviceProvider.GetServices<IPermissionProvider>();
            Initialize();
        }
        protected ISignal Signal { get; }
        protected IRoleStore RoleStore { get; }
        protected override CancellationToken CancellationToken => Signal.Token;
        protected IEnumerable<IPermissionProvider> PermissionProviders { get; }

        public List<PermissionDefinition> Permissions { get; } = new List<PermissionDefinition>();

        private void Initialize()
        {
            if (PermissionProviders != null)
            {
                foreach (var provider in PermissionProviders)
                {
                    Permissions.AddRange(provider.Definitions());
                }
            }
        }

        public async Task<List<PermissionDefinition>> GetPermissionsAsync(Role role)
        {
            if (PermissionProviders == null)
                return null;
            var permissions = await RoleStore.GetPermissionClaimsAsync(role, CancellationToken);

            if (permissions != null)
                RecursivelyPermission(Permissions, permissions);
            return Permissions;
        }

        private void RecursivelyPermission(List<PermissionDefinition> permissionDefinitions, IList<string> rolePermissions)
        {
            foreach (var permissionDefinition in permissionDefinitions)
            {
                var rolePermission = rolePermissions.FirstOrDefault(f => f == permissionDefinition.Name);
                if (rolePermission != null)
                    permissionDefinition.IsGrant = true;
                if (permissionDefinition.Children.Count > 0)
                {
                    RecursivelyPermission(permissionDefinition.Children.ToList(), rolePermissions);
                }
            }
        }
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
                var permissionClaims = claims.Where(w => w.Type == AppConstants.ClaimTypes.Permission).ToList();
                foreach (var permissionClaim in permissionClaims)
                {
                    await RoleStore.RemoveClaimAsync(role, permissionClaim, CancellationToken);
                }
            }
            // 增加权限Claim
            foreach (var permission in permissions)
            {
                await RoleStore.AddClaimAsync(role, new Claim(AppConstants.ClaimTypes.Permission, permission), CancellationToken);
            }

            return await UpdateRoleAsync(role);

        }

    }
}
