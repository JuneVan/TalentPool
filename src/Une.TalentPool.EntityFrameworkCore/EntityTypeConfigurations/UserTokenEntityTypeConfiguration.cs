using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Une.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    class UserTokenEntityTypeConfiguration : IEntityTypeConfiguration<IdentityUserToken<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder)
        {
            builder.ToTable("UserTokens");
            builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
             
            builder.Property(t => t.LoginProvider).HasMaxLength(128);
            builder.Property(t => t.Name).HasMaxLength(128);
        }
    }
}
