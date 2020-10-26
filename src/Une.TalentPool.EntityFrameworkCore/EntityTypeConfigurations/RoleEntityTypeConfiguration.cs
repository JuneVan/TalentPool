using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Une.TalentPool.Roles;

namespace Une.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.Property(p => p.DisplayName).HasMaxLength(256);
            builder.Property(p => p.Description).HasMaxLength(512);

            

            builder.HasKey(r => r.Id);
            builder.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex").IsUnique();
            builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

            builder.Property(u => u.Name).HasMaxLength(256);
            builder.Property(u => u.NormalizedName).HasMaxLength(256);

            builder.HasMany<IdentityUserRole<Guid>>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
            builder.HasMany<IdentityRoleClaim<Guid>>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();

            builder.HasQueryFilter(f => !f.IsDeleted);
        }
    }
}
