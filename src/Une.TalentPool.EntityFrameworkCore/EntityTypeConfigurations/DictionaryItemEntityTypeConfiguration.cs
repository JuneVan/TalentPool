using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Une.TalentPool.Dictionaries;

namespace Une.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    internal class DictionaryItemEntityTypeConfiguration : IEntityTypeConfiguration<DictionaryItem>
    {
        public void Configure(EntityTypeBuilder<DictionaryItem> builder)
        {
            builder.ToTable("DictionaryItems");
            builder.Property(p => p.Name).HasMaxLength(128); 
        }
    }
}
