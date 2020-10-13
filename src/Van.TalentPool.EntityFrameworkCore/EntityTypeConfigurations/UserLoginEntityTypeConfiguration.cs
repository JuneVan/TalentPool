using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Van.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    class UserLoginEntityTypeConfiguration : IEntityTypeConfiguration<IdentityUserLogin<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> builder)
        {
            builder.ToTable("UserLogins");
        }
    }
}
