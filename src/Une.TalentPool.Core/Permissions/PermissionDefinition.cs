using System.Collections.Generic;

namespace Une.TalentPool.Permissions
{
    public class PermissionDefinition
    {
        public string Name { get; private set; }
        public string DisplayName { get; private set; }
        public string Description { get; private set; }
        public bool IsGrant { get; set; }
        public int Level { get; private set; } = 0;
        public IReadOnlyList<PermissionDefinition> Children => _children;
        private List<PermissionDefinition> _children;
        public PermissionDefinition(string name, string displayName = null, string description = null)
        {
            Name = name;
            DisplayName = displayName;
            Description = description;
            _children = new List<PermissionDefinition>();
        }

        public PermissionDefinition AddChild(string name, string displayName = null, string description = null)
        {
            var permission = new PermissionDefinition(name, displayName, description);
            permission.Level = Level + 1;
            _children.Add(permission);
            return permission;
        }
    }
}
