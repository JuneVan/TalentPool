using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Une.TalentPool.Dictionaries;

namespace Une.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    internal class DictionaryEntityTypeConfiguration : IEntityTypeConfiguration<Dictionary>
    {
        public void Configure(EntityTypeBuilder<Dictionary> builder)
        {
            builder.ToTable("Dictionaries");
            builder.HasIndex(i => i.Name).HasName("NameIndex").IsUnique();
            builder.Property(p => p.Name).HasMaxLength(128);
            builder.Property(p => p.DisplayName).HasMaxLength(256);

            builder.HasMany(m => m.DictionaryItems).WithOne().HasForeignKey(fk => fk.DictionaryId);
        }
    }
}
