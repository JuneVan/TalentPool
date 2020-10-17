using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Une.TalentPool.Interviews;

namespace Une.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    class InterviewEntityTypeConfiguration : IEntityTypeConfiguration<Interview>
    {
        public void Configure(EntityTypeBuilder<Interview> builder)
        {
            builder.ToTable("Interviews");
            builder.HasKey(k => k.Id); 
            builder.Property(p => p.Remark).HasMaxLength(256);  
            builder.Property(p => p.Name).HasMaxLength(256);  
            builder.HasQueryFilter(f => !f.IsDeleted);//未删除的数据
        }
    }
}
