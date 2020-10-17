using System.Collections.Generic;

namespace Une.TalentPool.Navigations
{
    public interface INavigationProvider
    {
        IEnumerable<NavigationDefinition> Definitions();
    }
}
