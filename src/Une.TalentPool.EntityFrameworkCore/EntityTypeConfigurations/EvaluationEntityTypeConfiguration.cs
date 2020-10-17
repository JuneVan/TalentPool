using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Une.TalentPool.Evaluations;

namespace Une.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    class EvaluationEntityTypeConfiguration : IEntityTypeConfiguration<Evaluation>
    {
        public void Configure(EntityTypeBuilder<Evaluation> builder)
        {
            builder.ToTable("Evaluations");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Title).HasMaxLength(128); 
            builder.HasQueryFilter(f => !f.IsDeleted);//未删除的数据

            builder.HasMany(m => m.Subjects).WithOne().HasForeignKey(fk => fk.EvaluationId);
            builder.HasMany(m => m.Questions).WithOne().HasForeignKey(fk => fk.EvaluationId);
        }
    }
}
