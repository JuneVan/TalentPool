using System;

namespace TalentPool.EntityFrameworkCore.Seeds
{
    public class SeedHelper
    {
        private readonly IServiceProvider _serviceProvider;
        public SeedHelper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void SeedDb()
        {
            new DefaultRoleAndUserCreator(_serviceProvider).Create().Wait(); 
        }

    }
}
