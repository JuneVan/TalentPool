using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentPool.Resumes;

namespace TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    class ResumeKeyMapEntityTypeConfiguration: IEntityTypeConfiguration<ResumeKeywordMap>
    { 
        public void Configure(EntityTypeBuilder<ResumeKeywordMap> builder)
        {
            builder.ToTable("ResumeKeyMaps");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Keyword).HasMaxLength(128);
            builder.HasIndex(i => i.Keyword).HasName("Index_Keyword");

            builder.HasQueryFilter(f => !f.IsDeleted);
        }
    }
}
