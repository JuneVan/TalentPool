using System;
using System.Collections.Generic;
using System.Text;

namespace Une.TalentPool.Navigations
{
    public class NavigationDefinition
    {
        public string Text { get; private set; }
        public string LinkUrl { get; private set; }
        public string Icon { get; private set; }
        public string PermissionName { get; set; }
        public IReadOnlyList<NavigationDefinition> Children => _children;
        private List<NavigationDefinition> _children;

        public NavigationDefinition(string text, string linkUrl, string icon = null, string permissionName = null)
        {
            Text = text;
            LinkUrl = linkUrl;
            Icon = icon;
            PermissionName = permissionName;
            _children = new List<NavigationDefinition>();
        }
        public NavigationDefinition AddChild(string text, string linkUrl, string icon = null, string permissionName = null)
        {
            var navigation = new NavigationDefinition(text, linkUrl, icon, permissionName);
            _children.Add(navigation);
            return navigation;
        }
    }
}
