using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Van.TalentPool.Roles;

namespace Van.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.Property(p => p.DisplayName).HasMaxLength(256);
            builder.Property(p => p.Description).HasMaxLength(512);

            builder.HasQueryFilter(f => !f.IsDeleted);
        }
    }
}
