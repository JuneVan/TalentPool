using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Van.TalentPool.Configurations;
using Van.TalentPool.Entities;
using Van.TalentPool.EntityFrameworkCore.EntityTypeConfigurations;
using Van.TalentPool.Roles;
using Van.TalentPool.Users;

namespace Van.TalentPool.EntityFrameworkCore
{
    public class VanDbContext : IdentityDbContext<User, Role, Guid>
    {

        public VanDbContext(DbContextOptions options, IServiceProvider serviceProvider) : base(options)
        {
            UserIdentifier = serviceProvider.GetService<IUserIdentifier>();
        }
        public VanDbContext(IServiceProvider serviceProvider)
        {
            UserIdentifier = serviceProvider.GetService<IUserIdentifier>();
        }

        public DbSet<SettingValue> SettingValues { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            builder.ApplyConfiguration(new RoleClaimEntityTypeConfiguration());
            builder.ApplyConfiguration(new UserEntityTypeConfiguration());
            builder.ApplyConfiguration(new UserRoleEntityTypeConfiguration());
            builder.ApplyConfiguration(new UserLoginEntityTypeConfiguration());
            builder.ApplyConfiguration(new UserTokenEntityTypeConfiguration());
            builder.ApplyConfiguration(new UserClaimEntityTypeConfiguration());
        }

        protected IUserIdentifier UserIdentifier { get; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        {
                            IHasCreationTime hasCreation = entry.Entity as IHasCreationTime;
                            if (hasCreation == null)
                                continue;
                            hasCreation.CreationTime = DateTime.Now;

                            ICreationAudited creationAudited = entry.Entity as ICreationAudited;
                            if (creationAudited == null)
                                continue;
                            if (UserIdentifier != null && UserIdentifier.UserId.HasValue)
                                creationAudited.CreatorUserId = UserIdentifier.UserId.Value;

                        }
                        break;
                    case EntityState.Modified:
                        {
                            IHasModificationTime hasModification = entry.Entity as IHasModificationTime;
                            if (hasModification == null)
                                continue;
                            hasModification.LastModificationTime = DateTime.Now;

                            IModificationAudited modificationAudited = entry.Entity as IModificationAudited;
                            if (modificationAudited == null)
                                continue;
                            if (UserIdentifier != null && UserIdentifier.UserId.HasValue)
                                modificationAudited.LastModifierUserId = UserIdentifier.UserId.Value;
                        }
                        break;
                    case EntityState.Deleted:
                        {

                            IHasDeletionTime hasDeletion = entry.Entity as IHasDeletionTime;
                            if (hasDeletion == null)
                                continue;
                            entry.Reload();
                            entry.State = EntityState.Modified;

                            hasDeletion.DeletionTime = DateTime.Now;
                            hasDeletion.IsDeleted = true;

                            IDeletionAudited deletionAudited = entry.Entity as IDeletionAudited;
                            if (deletionAudited == null)
                                continue;
                            if (UserIdentifier != null && UserIdentifier.UserId.HasValue)
                                deletionAudited.DeleterUserId = UserIdentifier.UserId.Value;

                        }
                        break;
                }
            }


            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
