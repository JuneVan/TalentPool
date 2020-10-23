using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Une.TalentPool.Users;

namespace Une.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.Property(p => p.Name).HasMaxLength(256);
            builder.Property(p => p.Surname).HasMaxLength(256);
            builder.Property(p => p.Photo).HasMaxLength(2048); 
            builder.Ignore(i => i.FullName);

            builder.HasQueryFilter(f => !f.IsDeleted);
        }
    }
}
