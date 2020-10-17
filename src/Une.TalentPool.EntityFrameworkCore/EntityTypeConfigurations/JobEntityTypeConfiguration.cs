using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Une.TalentPool.Jobs;

namespace Une.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    class JobEntityTypeConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.ToTable("Jobs");
            builder.Property(p => p.Title).HasMaxLength(256);
            builder.Property(p => p.Requirements).HasMaxLength(2048);
            builder.Property(p => p.Description).HasMaxLength(2048);
            builder.Property(p => p.Keywords).HasMaxLength(512);
            builder.Property(p => p.SalaryRange).HasMaxLength(128);
            builder.Property(p => p.GenderRange).HasMaxLength(128);
            builder.Property(p => p.AgeRange).HasMaxLength(128);
            builder.Property(p => p.Remark).HasMaxLength(512);
            builder.Property(p => p.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
