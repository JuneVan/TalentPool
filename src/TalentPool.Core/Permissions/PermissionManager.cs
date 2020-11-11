using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace TalentPool.Permissions
{
    public class PermissionManager
    {
        public PermissionManager(IServiceProvider serviceProvider)
        {
            PermissionProviders = serviceProvider.GetServices<IPermissionProvider>();
            Initialize();
        }

        protected IEnumerable<IPermissionProvider> PermissionProviders { get; }

        public List<PermissionDefinition> Permissions { get; } = new List<PermissionDefinition>();

        public void Initialize()
        {
            if (PermissionProviders != null)
            {
                foreach (var provider in PermissionProviders)
                {
                    Permissions.AddRange(provider.Definitions());
                }
            }
        }

        public List<PermissionDefinition> GetPermissionsTree(List<string> permissions)
        {
            if (PermissionProviders == null)
                return null;
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
    }
}
