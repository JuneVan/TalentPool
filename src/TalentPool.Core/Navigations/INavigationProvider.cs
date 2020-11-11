using System.Collections.Generic;

namespace TalentPool.Navigations
{
    public interface INavigationProvider
    {
        IEnumerable<NavigationDefinition> Definitions();
    }
}
