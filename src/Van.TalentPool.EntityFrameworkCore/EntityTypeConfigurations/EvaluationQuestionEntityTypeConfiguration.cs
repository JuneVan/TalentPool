using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Van.TalentPool.Evaluations;

namespace Van.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    class EvaluationQuestionEntityTypeConfiguration : IEntityTypeConfiguration<EvaluationQuestion>
    {
        public void Configure(EntityTypeBuilder<EvaluationQuestion> builder)
        {
            builder.ToTable("EvaluationQuestions");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Description).HasMaxLength(1024); 
            builder.Property(p => p.ReferenceAnswer).HasMaxLength(3072);
        }
    }
}
