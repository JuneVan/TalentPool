using System;

namespace Van.TalentPool.Permissions
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class PermissionCheckAttribute : Attribute
    {
        public string Permission { get; }
        public PermissionCheckAttribute(string permission)
        {
            Permission = permission;
        }
    }
}
