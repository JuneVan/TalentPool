using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentPool.Resumes;

namespace TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    class ResumeAttachmentEntityTypeConfiguration : IEntityTypeConfiguration<ResumeAttachment>
    {
        public void Configure(EntityTypeBuilder<ResumeAttachment> builder)
        {
            builder.ToTable("ResumeAttachments");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.FileName).HasMaxLength(128);
            builder.Property(p => p.FilePath).HasMaxLength(2048);
        }
    }
}
