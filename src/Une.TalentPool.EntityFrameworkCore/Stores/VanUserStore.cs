using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using Une.TalentPool.Roles;
using Une.TalentPool.Users;

namespace Une.TalentPool.EntityFrameworkCore.Stores
{
    public class VanUserStore : UserStore<User, Role, VanDbContext, Guid>, IUserStore
    {
        public VanUserStore(VanDbContext context) : base(context)
        {

        }
    }
}
