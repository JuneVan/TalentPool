using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Une.TalentPool.Navigations
{
    public class NavigationManager
    {
        public NavigationManager(IServiceProvider serviceProvider)
        {
            NavigationProviders = serviceProvider.GetServices<INavigationProvider>();
            Initialize();
        }
        protected IEnumerable<INavigationProvider> NavigationProviders { get; }
        public List<NavigationDefinition> Navigations { get; } = new List<NavigationDefinition>();

        public void Initialize()
        {
            if (NavigationProviders != null)
            {
                foreach (var provider in NavigationProviders)
                {
                    Navigations.AddRange(provider.Definitions());
                }
            }
        }
    }
}
