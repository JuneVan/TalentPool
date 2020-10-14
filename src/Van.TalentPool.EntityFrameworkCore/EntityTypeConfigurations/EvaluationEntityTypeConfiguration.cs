using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Van.TalentPool.Evaluations;

namespace Van.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    class EvaluationEntityTypeConfiguration : IEntityTypeConfiguration<Evaluation>
    {
        public void Configure(EntityTypeBuilder<Evaluation> builder)
        {
            builder.ToTable("Evaluations");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Title).HasMaxLength(128); 
            builder.HasQueryFilter(f => !f.IsDeleted);//未删除的数据
        }
    }
}
