using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Une.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    class RoleClaimEntityTypeConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
        {
            builder.ToTable("RoleClaims");
            builder.HasKey(rc => rc.Id);
        }
    }
}
