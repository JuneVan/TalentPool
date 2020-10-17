using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Une.TalentPool.Resumes;

namespace Une.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    class ResumeCompareEntityTypeConfiguration : IEntityTypeConfiguration<ResumeCompare>
    {
        public void Configure(EntityTypeBuilder<ResumeCompare> builder)
        {
            builder.ToTable("ResumeCompares");
            builder.HasKey(k => k.Id);
        }
    }
}
