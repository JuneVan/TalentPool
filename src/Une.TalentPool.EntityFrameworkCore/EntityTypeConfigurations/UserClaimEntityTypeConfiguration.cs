using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Une.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    public class UserClaimEntityTypeConfiguration : IEntityTypeConfiguration<IdentityUserClaim<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
        {
            builder.ToTable("UserClaims");

            builder.HasKey(uc => uc.Id);
        }
    }
}
