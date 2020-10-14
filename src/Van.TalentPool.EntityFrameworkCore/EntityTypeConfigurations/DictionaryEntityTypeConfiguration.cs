using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Van.TalentPool.Dictionaries;

namespace Van.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    internal class DictionaryEntityTypeConfiguration : IEntityTypeConfiguration<Dictionary>
    {
        public void Configure(EntityTypeBuilder<Dictionary> builder)
        {
            builder.ToTable("Dictionaries");
            builder.HasIndex(i => i.Name).HasName("NameIndex").IsUnique();
            builder.Property(p => p.Name).HasMaxLength(128);
            builder.Property(p => p.DisplayName).HasMaxLength(256);
        }
    }
}
