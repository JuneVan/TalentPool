using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Une.TalentPool.Resumes;

namespace Une.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    class ResumeAuditSettingEntityTypeConfiguration : IEntityTypeConfiguration<ResumeAuditSetting>
    {
        public void Configure(EntityTypeBuilder<ResumeAuditSetting> builder)
        {
            builder.ToTable("ResumeAuditSettings");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.UserName).HasMaxLength(32);
        }
    }
}
