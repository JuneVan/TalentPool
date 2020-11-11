using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentPool.Resumes;

namespace TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    class ResumeAuditRecordEntityTypeConfiguration : IEntityTypeConfiguration<ResumeAuditRecord>
    {
        public void Configure(EntityTypeBuilder<ResumeAuditRecord> builder)
        {
            builder.ToTable("ResumeAuditRecords");
            builder.HasKey(k => k.Id); 
            builder.Property(p => p.Remark).HasMaxLength(1024);
        }
    }
}
