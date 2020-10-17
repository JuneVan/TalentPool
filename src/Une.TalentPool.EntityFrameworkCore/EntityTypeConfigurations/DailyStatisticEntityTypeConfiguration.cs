using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Une.TalentPool.DailyStatistics;

namespace Une.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    public class DailyStatisticEntityTypeConfiguration : IEntityTypeConfiguration<DailyStatistic>
    {
        public void Configure(EntityTypeBuilder<DailyStatistic> builder)
        {
            builder.ToTable("DailyStatistics");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Platform).HasMaxLength(128);
            builder.Property(p => p.Description).HasMaxLength(512);
            builder.HasMany(m => m.Items).WithOne().HasForeignKey(fk => fk.DailyStatisticId);
        }
    }
}
