using System.Collections.Generic;

namespace Van.TalentPool.Navigations
{
    public interface INavigationProvider
    {
        IEnumerable<NavigationDefinition> Definitions();
    }
}
