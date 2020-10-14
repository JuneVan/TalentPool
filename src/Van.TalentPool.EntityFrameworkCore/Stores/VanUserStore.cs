using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using Van.TalentPool.Roles;
using Van.TalentPool.Users;

namespace Van.TalentPool.EntityFrameworkCore.Stores
{
    public class VanUserStore : UserStore<User, Role, VanDbContext, Guid>, IUserStore
    {
        public VanUserStore(VanDbContext context) : base(context)
        {

        }
    }
}
