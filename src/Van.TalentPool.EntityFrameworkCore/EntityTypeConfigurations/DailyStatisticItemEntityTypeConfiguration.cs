using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Van.TalentPool.DailyStatistics;

namespace Van.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    public class DailyStatisticItemEntityTypeConfiguration : IEntityTypeConfiguration<DailyStatisticItem>
    {
        public void Configure(EntityTypeBuilder<DailyStatisticItem> builder)
        {
            builder.ToTable("DailyStatisticItems");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.JobName).HasMaxLength(128); 
        }
    }
}
