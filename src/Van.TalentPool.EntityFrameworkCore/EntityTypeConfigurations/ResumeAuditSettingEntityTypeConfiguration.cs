using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Van.TalentPool.Resumes;

namespace Van.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
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
